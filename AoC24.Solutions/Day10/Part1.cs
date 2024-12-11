using AoC24.Solutions.Day10.Models;
using BenchmarkDotNet.Attributes;
using System.Linq;

namespace AoC24.Solutions.Day10;

/* BENCHMARK RESULTS
// * Summary *

BenchmarkDotNet v0.14.0, Windows 10 (10.0.19045.5131/22H2/2022Update)
11th Gen Intel Core i7-1185G7 3.00GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK 9.0.101
  [Host]   : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0 : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 9.0 : .NET 9.0.0 (9.0.24.52809), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


| Method | Job      | Runtime  | Mean     | Error    | StdDev   | Allocated |
|------- |--------- |--------- |---------:|---------:|---------:|----------:|
| Dfs    | .NET 8.0 | .NET 8.0 | 685.1 ms | 13.68 ms | 15.75 ms |   3.43 MB |
| Bfs    | .NET 8.0 | .NET 8.0 | 677.6 ms | 13.55 ms | 17.13 ms |   3.43 MB |
| Dfs    | .NET 9.0 | .NET 9.0 | 567.0 ms | 11.08 ms | 11.85 ms |   3.39 MB |
| Bfs    | .NET 9.0 | .NET 9.0 | 562.6 ms | 11.17 ms | 13.30 ms |   3.39 MB |
*/

[MemoryDiagnoser]
[SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net80)]
[SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net90)]
public partial class Part1(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\1001.txt";
    private readonly string _filePath = filePath;

    public Part1()
        : this(FilePath)
    { }

    public long GetResult() => Method3();

    // Depth-first search
    public long Method1() => Dfs().Distinct().Count();
    // Breadth-first search
    public long Method2() => Bfs().Distinct().Count();
    // Depth-first search, optimized
    public long Method3() => DfsOptimized().Distinct().Count();

    [Benchmark]
    public List<(Point, Point)> Dfs()
    {
        var grid = File.ReadAllLines(_filePath)
            .SelectMany((line, y) => line.Select((c, x) => new GridTile(new Point(x, y), Convert.ToByte(c.ToString()))).ToArray())
            .ToDictionary((item) => item.Position, (item) => item);

        var startingPoints = grid.Where((item) => item.Value.Height == 0).ToDictionary((item) => item.Key, (item) => item.Value);

        var results = new List<(Point, Point)>();
        foreach (var startingPoint in startingPoints)
        {
            var frontier = new Stack<GridTile>([startingPoint.Value]);
            while (frontier.Count > 0)
            {
                var current = frontier.Pop();
                var adjacentPositions = new Point[]
                {
                    new Point(current.Position.X - 1, current.Position.Y),
                    new Point(current.Position.X + 1, current.Position.Y),
                    new Point(current.Position.X, current.Position.Y - 1),
                    new Point(current.Position.X, current.Position.Y + 1),
                };
                foreach (var adjacent in grid.Where((item) => adjacentPositions.Contains(item.Key) && item.Value.Height == current.Height + 1))
                {
                    if (adjacent.Value.Height != 9)
                    {
                        frontier.Push(adjacent.Value);
                    }
                    else
                    {
                        // Reached an endpoint
                        results.Add((startingPoint.Value.Position, adjacent.Value.Position));
                    }
                }
            }
        }

        return results;
    }

    [Benchmark]
    public IEnumerable<(Point, Point)> DfsOptimized()
    {
        var grid = File.ReadAllLines(_filePath)
            .Select((line, y) => line.Select((c, x) => new GridTile(new Point(x, y), Convert.ToByte(c.ToString()))).ToArray())
            .ToArray();

        var startingPoints = grid.SelectMany((item) => item).Where((item) => item.Height == 0).ToArray();
        var width = grid[0].Length;
        var height = grid.Length;

        foreach (var startingPoint in startingPoints)
        {
            var frontier = new Stack<GridTile>([startingPoint]);
            while (frontier.Count > 0)
            {
                var current = frontier.Pop();
                var adjacentPositions = new List<GridTile>();
                if (current.Position.X > 0)
                    adjacentPositions.Add(grid[current.Position.Y][current.Position.X - 1]);
                if (current.Position.X < (width - 1))
                    adjacentPositions.Add(grid[current.Position.Y][current.Position.X + 1]);
                if (current.Position.Y > 0)
                    adjacentPositions.Add(grid[current.Position.Y - 1][current.Position.X]);
                if (current.Position.Y < (height - 1))
                    adjacentPositions.Add(grid[current.Position.Y + 1][current.Position.X]);

                foreach (var adjacent in adjacentPositions.Where((item) => item.Height == current.Height + 1))
                {
                    if (adjacent.Height != 9)
                    {
                        frontier.Push(adjacent);
                    }
                    else
                    {
                        // Reached an endpoint
                        yield return (startingPoint.Position, adjacent.Position);
                    }
                }
            }
        }
    }

    [Benchmark]
    public List<(Point, Point)> Bfs()
    {
        var grid = File.ReadAllLines(_filePath)
            .SelectMany((line, y) => line.Select((c, x) => new GridTile(new Point(x, y), Convert.ToByte(c.ToString()))).ToArray())
            .ToDictionary((item) => item.Position, (item) => item);

        var startingPoints = grid.Where((item) => item.Value.Height == 0).ToDictionary((item) => item.Key, (item) => item.Value);

        var results = new List<(Point, Point)>();
        foreach (var startingPoint in startingPoints)
        {
            var frontier = new Queue<GridTile>([startingPoint.Value]);
            while (frontier.Count > 0)
            {
                var current = frontier.Dequeue();
                var adjacentPositions = new Point[]
                {
                    new Point(current.Position.X - 1, current.Position.Y),
                    new Point(current.Position.X + 1, current.Position.Y),
                    new Point(current.Position.X, current.Position.Y - 1),
                    new Point(current.Position.X, current.Position.Y + 1),
                };
                foreach (var adjacent in grid.Where((item) => adjacentPositions.Contains(item.Key) && item.Value.Height == current.Height + 1))
                {
                    if (adjacent.Value.Height != 9)
                    {
                        frontier.Enqueue(adjacent.Value);
                    }
                    else
                    {
                        results.Add((startingPoint.Value.Position, adjacent.Value.Position));
                    }
                }
            }
        }

        return results;
    }
}
