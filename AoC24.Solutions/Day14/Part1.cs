using System.Text.RegularExpressions;

namespace AoC24.Solutions.Day14;

public partial class Part1(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\1401.txt";
    private readonly string _filePath = filePath;

    [GeneratedRegex(@"p\=(?<p_x>\-?\d+)\,(?<p_y>\-?\d+)\s+v\=(?<v_x>\-?\d+)\,(?<v_y>\-?\d+)")]
    internal static partial Regex RobotPattern();

    public int Seconds { get; }
    public int Width { get; }
    public int Height { get; }

    public Part1()
        : this(FilePath, 100, 101,103)
    { }

    public Part1(int seconds, int width, int height)
        : this(FilePath, seconds, width, height)
    { }

    public Part1(string filePath, int seconds, int width, int height)
        : this(filePath)
    {
        Seconds = seconds;
        Width = width;
        Height = height;
    }

    public long GetResult()
    {
        var robots = File.ReadAllLines(_filePath)
            .Select((line) => RobotPattern().Match(line) is Match match ?
                new
                {
                    Position = new { X = Convert.ToInt32(match.Groups["p_x"].Value), Y = Convert.ToInt32(match.Groups["p_y"].Value) },
                    Velocity = new { X = Convert.ToInt32(match.Groups["v_x"].Value), Y = Convert.ToInt32(match.Groups["v_y"].Value) },
                } :
                null)
            .Where((item) => item != null)
            .ToArray();

        var movedRobots = robots.Select((robot) => new
        {
            Robot = robot,
            NewPosition = new
            {
                X = ((Width + (robot!.Position.X + ((robot.Velocity.X * Seconds) % Width))) % Width),
                Y = ((Height + (robot.Position.Y + ((robot.Velocity.Y * Seconds) % Height))) % Height),
            },
        });

        var quadrants = Enumerable.Range(0, 4).Select((index) => new
        {
            Number = index + 1,
            Bounds = new
            {
                Top = (index % 2) * ((Height + 1) / 2),
                Right = ((index / 2) * ((Width + 1) / 2)) + ((Width - 1) / 2) - 1,
                Bottom = ((index % 2) * ((Height + 1) / 2)) + ((Height - 1) / 2) - 1,
                Left = (index / 2) * ((Width + 1) / 2),
            }
        });

        var results = quadrants.Select(
            (quadrant) => movedRobots.Where(
                (robot) =>
                    robot!.NewPosition.X >= quadrant.Bounds.Left && robot.NewPosition.X <= quadrant.Bounds.Right
                    && robot!.NewPosition.Y >= quadrant.Bounds.Top && robot.NewPosition.Y <= quadrant.Bounds.Bottom
                ).ToArray()
            ).ToArray();

        return results.Aggregate(1, (aggregate, quadrant) => aggregate * quadrant.Count());
    }
}
