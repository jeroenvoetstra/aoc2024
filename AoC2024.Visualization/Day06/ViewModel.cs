using AoC2024.Visualization.Day06.Models;
using AoC24.Solutions.Day06.Models;
using Microsoft.Diagnostics.Runtime.Utilities;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace AoC2024.Visualization.Day06;

public class ViewModel : ViewModelBase
{
    private const int StepWait = 1000;

    private Laboratory _lab;
    private Guard _guard;

    private List<List<TileState>> _originalGrid;

    private string _currentPosition;

    public ObservableCollection<ObservableCollection<TileState>> Grid { get; }

    public string CurrentPosition
    {
        get { return _currentPosition; }
        set { SetValue(ref _currentPosition, value); }
    }

    public ViewModel()
    {
        //var random = new Random();
        //for (var i = 0; i < 10; i++)
        //{
        //    var collection = new ObservableCollection<int>(new int[10]);
        //    Numbers.Add(collection);
        //}
        //var timer = new System.Timers.Timer(100);
        //timer.Elapsed += (sender, e) =>
        //{
        //    Synchronize(() => Numbers[random.Next(0, 10)][random.Next(0, 10)] += 1);
        //};
        //timer.Start();

        var input = File.ReadAllLines(@"..\..\..\..\Input\0601.test.txt");
        _lab = new Laboratory(
            input
                .Select((line, y) =>
                    line
                        .Select((c, x) =>
                        {
                            if (Guard.GuardOrientations.ContainsKey(c))
                                _guard = new Guard(c, x, y);

                            return new Tile(c, x, y);
                        })
                        .ToArray()
                    )
                .ToArray()
            );

        if (_guard == default)
            throw new InvalidOperationException();

        var grid = _lab.Grid.Select((line) => new ObservableCollection<TileState>(line.Select((tile) => Convert(tile.TileType))));
        _originalGrid = new List<List<TileState>>(grid.Select((item) => item.ToList()).ToList());
        Grid = new ObservableCollection<ObservableCollection<TileState>>(grid);
        Grid[_guard.Position.Y][_guard.Position.X] = TileState.Player;
        CurrentPosition = $"{_guard.Position.X}, {_guard.Position.Y}";
        Task.Run(StartScenario2);
    }

    private async Task StartScenario1()
    {
        var previousGuardPosition = _guard.Position;
        foreach (var step in _guard.GetRouteUntilOut(_lab))
        {
            Synchronize(() =>
            {
                Grid[previousGuardPosition.Y][previousGuardPosition.X] = _originalGrid[previousGuardPosition.Y][previousGuardPosition.X];
                Grid[_guard.Position.Y][_guard.Position.X] = TileState.Player;

                previousGuardPosition = _guard.Position;
            });

            await Task.Delay(StepWait);
        }
    }

    private async Task StartScenario2()
    {
        var previousGuardPosition = _guard.Position;
        foreach (var (tile, orientation) in _guard.GetRouteUntilOut(_lab))
        {
            Synchronize(() =>
            {
                CurrentPosition = $"{_guard.Position.X}, {_guard.Position.Y}";

                Grid[previousGuardPosition.Y][previousGuardPosition.X] = _originalGrid[previousGuardPosition.Y][previousGuardPosition.X];
                Grid[_guard.Position.Y][_guard.Position.X] = TileState.Player;

                previousGuardPosition = _guard.Position;
            });

            var nextCoordinate = tile.Position + orientation;
            if (!_lab.IsInBounds(nextCoordinate))
                continue;

            var nextTile = _lab.Grid[nextCoordinate.Y][nextCoordinate.X];
            if (nextTile.TileType == TileType.Obstacle)
                continue;

            // Store the orientation and position to revert back to later.
            var (previousPosition, previousOrientation) = (_guard.Position, _guard.Orientation);
            // Also store the original tile type.
            var originalType = nextTile.TileType;

            // Change tile and check if the guard will end up in a loop.
            nextTile.ChangeType(TileType.Obstacle);
            // Check if there's even a need to simulate, if there's no obstacle in the new direction
            // there will be no need to even check.
            if (_lab.HasObstacleInDirection(_guard.Position, Vector.TurnClockwise(_guard.Orientation)))
            {
                Synchronize(() =>
                {
                    Grid[nextTile.Position.Y][nextTile.Position.X] = TileState.TemporaryObstacle;
                });

                var result = false;
                var localVisitedCache = new List<(Point position, Vector orientation)>();
                foreach (var item in _guard.GetRouteUntilOut(_lab))
                {
                    await Task.Delay(StepWait);

                    Synchronize(() =>
                    {
                        CurrentPosition = $"{_guard.Position.X}, {_guard.Position.Y}";

                        Grid[previousGuardPosition.Y][previousGuardPosition.X] = _originalGrid[previousGuardPosition.Y][previousGuardPosition.X];
                        Grid[_guard.Position.Y][_guard.Position.X] = TileState.TemporaryPlayer;

                        previousGuardPosition = _guard.Position;
                    });

                    //if (localVisitedCache.Any(((Point position, Vector orientation) item) => Position == item.position && Orientation == item.orientation))
                    if (localVisitedCache.Contains((item.Tile.Position, item.Orientation)))
                        result = true;
                    localVisitedCache.Add((item.Tile.Position, item.Orientation));
                }

                if (result)
                {
                    await Task.Delay(5000);
                    Synchronize(() =>
                    {
                        Grid[nextTile.Position.Y][nextTile.Position.X] = TileState.Obstacle;
                    });
                }

                Synchronize(() =>
                {
                    Grid[nextTile.Position.Y][nextTile.Position.X] = _originalGrid[nextTile.Position.Y][nextTile.Position.X];
                });

                //results.Add(nextTile.Position);
            }

            // Reset back to original maze, and put the guard back at the last processed corner.
            nextTile.ChangeType(originalType);
            _guard.ResetTo(previousPosition, previousOrientation);

            await Task.Delay(StepWait);
        }

    }

    private static TileState Convert(TileType tileType)
        => tileType switch
        {
            TileType.FreeSpace => TileState.Empty,
            TileType.Obstacle => TileState.Obstacle,
            _ => throw new ArgumentException(nameof(tileType)),
        };
}
