using System.Collections.ObjectModel;
using System.IO;
using System.Text.RegularExpressions;

namespace AoC2024.Visualization.Day14;

public partial class ViewModel : ViewModelBase
{
    private static readonly string FilePath = @"Input\1401.txt";

    private const int Height = 103;
    private const int Width = 101;

    private const int OffsetX = 35;
    private const int OffsetY = 35;
    private const double Threshold = 0.5d;

    [GeneratedRegex(@"p\=(?<p_x>\-?\d+)\,(?<p_y>\-?\d+)\s+v\=(?<v_x>\-?\d+)\,(?<v_y>\-?\d+)")]
    internal static partial Regex RobotPattern();

    private List<Robot> _robots;
    private System.Timers.Timer _timer;
    private int _seconds;
    private bool _isStarted;
    private int _alertThreshold;

    public ObservableCollection<ObservableCollection<bool>> Grid { get; }

    public int Seconds
    {
        get { return _seconds; }
        set
        {
            if (SetValue(ref _seconds, value))
            {
                UpdateRobots();
            }
        }
    }

    public bool IsStarted
    {
        get { return _isStarted; }
        set { SetValue(ref _isStarted, value); }
    }

    public RelayCommand StartCommand { get; private set; }
    public RelayCommand StopCommand { get; private set; }
    public RelayCommand BackCommand { get; private set; }
    public RelayCommand ForwardCommand { get; private set; }
    public RelayCommand ResetCommand { get; private set; }

    public ViewModel()
    {
        _robots = File.ReadAllLines(Path.Combine(Environment.GetEnvironmentVariable("AOC_HOME", EnvironmentVariableTarget.Process)!, FilePath))
            .Select((line) => RobotPattern().Match(line) is Match match ?
                new Robot(
                    new Vector(Convert.ToInt32(match.Groups["p_x"].Value), Convert.ToInt32(match.Groups["p_y"].Value)),
                    new Vector(Convert.ToInt32(match.Groups["v_x"].Value), Convert.ToInt32(match.Groups["v_y"].Value))
                    ) :
                null)
            .Where((item) => item != null)
            .Select((item) => item!)
            .ToList();

        var grid = Enumerable.Range(0, Height - (2 * OffsetY)).Select((_) => Enumerable.Range(0, Width - (2 * OffsetX)).Select((_) => false).ToArray()).ToArray();
        Grid = new ObservableCollection<ObservableCollection<bool>>(grid.Select((item) => new ObservableCollection<bool>(item)));

        _alertThreshold = (int)(((Height - (2 * OffsetY)) * (Width - (2 * OffsetX))) * Threshold);

        BackCommand = new RelayCommand(GoBack, CanGoBack);
        ForwardCommand = new RelayCommand(GoForward, CanGoForward);
        StartCommand = new RelayCommand(Start, CanStart);
        StopCommand = new RelayCommand(Stop, CanStop);
        ResetCommand = new RelayCommand(Reset, CanReset);

        UpdateRobots();

        _timer = new System.Timers.Timer(10);
        _timer.Elapsed += (sender, e) =>
        {
            if (!IsStarted)
                _timer.Stop();

            Synchronize(() =>
            {
                Seconds++;
            });
        };
    }

    protected override void Dispose(bool disposing)
    {
        _timer.Dispose();
        base.Dispose(disposing);
    }

    private void UpdateRobots()
    {
        foreach (var robot in _robots)
        {
            Synchronize(() =>
            {
                if (robot.CurrentPosition.X >= OffsetX && robot.CurrentPosition.X < Width - OffsetX
                    && robot.CurrentPosition.Y >= OffsetY && robot.CurrentPosition.Y < Height - OffsetY)
                {
                    Grid[robot.CurrentPosition.Y - OffsetY][robot.CurrentPosition.X - OffsetX] = false;
                }
            });

            var newPosition = new Vector(
                ((Width + (robot!.Position.X + ((robot.Velocity.X * Seconds) % Width))) % Width),
                ((Height + (robot.Position.Y + ((robot.Velocity.Y * Seconds) % Height))) % Height)
                );
            robot.CurrentPosition = newPosition;

            Synchronize(() =>
            {
                if (robot.CurrentPosition.X >= OffsetX && robot.CurrentPosition.X < Width - OffsetX
                    && robot.CurrentPosition.Y >= OffsetY && robot.CurrentPosition.Y < Height - OffsetY)
                {
                    Grid[robot.CurrentPosition.Y - OffsetY][robot.CurrentPosition.X - OffsetX] = true;
                }
            });
        }
        if (Grid.Sum((row) => row.Count((item) => item == true)) > _alertThreshold)
        {
            Stop();
        }
    }

    private bool CanReset() => Seconds != 0;
    private bool CanStart() => !IsStarted;
    private bool CanStop() => IsStarted;
    private bool CanGoBack() => Seconds < 10000;
    private bool CanGoForward() => Seconds >= 0;

    private void Reset() => Synchronize(() => Seconds = 0);
    private void Start()// => Synchronize(() => Seconds = 0);
    {
        Synchronize(() =>
        {
            IsStarted = true;
        });

        _timer.Start();
    }
    private void Stop()// => Synchronize(() => Seconds = 0);
    {
        _timer.Stop();

        Synchronize(() =>
        {
            IsStarted = false;
        });
    }
    private void GoBack() => Synchronize(() => Seconds--);
    private void GoForward() => Synchronize(() => Seconds++);

    public record Vector(int X, int Y)
    {
        public static Vector operator +(Vector left, Vector right) => new Vector(left.X + right.X, left.Y + right.Y);
        public static Vector operator -(Vector left, Vector right) => new Vector(left.X - right.X, left.Y - right.Y);
    }

    private class Robot(Vector position, Vector velocity)
    {
        public Vector Position { get; set; } = position;
        public Vector Velocity { get; set; } = velocity;

        public Vector CurrentPosition { get; set; } = position;
    }
}
