using System.Text.RegularExpressions;

namespace AoC24.Solutions.Day14;

public partial class Part2(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\1401.txt";
    private readonly string _filePath = filePath;

    [GeneratedRegex(@"p\=(?<p_x>\-?\d+)\,(?<p_y>\-?\d+)\s+v\=(?<v_x>\-?\d+)\,(?<v_y>\-?\d+)")]
    internal static partial Regex RobotPattern();

    // When there are more than this amount of robots clustered together (adjacent to each
    // other), we can assume it's what we're looking for.
    private const int Threshold = 30;

    public int Width { get; }
    public int Height { get; }

    public Part2()
        : this(FilePath, 101, 103)
    { }

    public Part2(int width, int height)
        : this(FilePath, width, height)
    { }

    public Part2(string filePath, int width, int height)
        : this(FilePath)
    {
        Width = width;
        Height = height;
    }

    public long GetResult()
    {
        var robots = File.ReadAllLines(_filePath)
            .Select((line) => RobotPattern().Match(line) is Match match ?
                new Robot(
                    new Vector(Convert.ToInt32(match.Groups["p_x"].Value), Convert.ToInt32(match.Groups["p_y"].Value)),
                    new Vector(Convert.ToInt32(match.Groups["v_x"].Value), Convert.ToInt32(match.Groups["v_y"].Value))
                    ) : null)
            .Where((item) => item != null)
            .Select((item) => item!)
            .ToArray();

        for (var i = 1; i < 10000; i++)
        {
            var currentSet = GetRobotsInPositionAfter(robots, i);
            var visited = new HashSet<Vector>();
            var biggestGroup = currentSet.Max((item) => GetGroupRecursive(currentSet, visited, item).Count());
            if (biggestGroup > Threshold)
                return i;
        }

        throw new ChristmasTreeNotFoundException();
    }

    private HashSet<Vector> GetRobotsInPositionAfter(Robot[] robots, int second)
    {
        return new HashSet<Vector>(robots.Select((robot) => new Vector(
            ((Width + (robot!.Position.X + ((robot.Velocity.X * second) % Width))) % Width),
            ((Height + (robot.Position.Y + ((robot.Velocity.Y * second) % Height))) % Height)
            )));
    }

    private IEnumerable<Vector> GetGroupRecursive(HashSet<Vector> positions, HashSet<Vector> visited, Vector node)
    {
        visited.Add(node);
        if (!visited.Contains(node))
        {
            yield return node;

            if (node.X > 0 && positions.Contains(new Vector(node.X - 1, node.Y)))
            {
                if (!visited.Contains(new Vector(node.X - 1, node.Y)))
                    foreach (var item in GetGroupRecursive(positions, visited, new Vector(node.X - 1, node.Y)))
                        yield return item;
            }
            if (node.X < Width - 1 && positions.Contains(new Vector(node.X + 1, node.Y)))
            {
                if (!visited.Contains(new Vector(node.X + 1, node.Y)))
                    foreach (var item in GetGroupRecursive(positions, visited, new Vector(node.X + 1, node.Y)))
                        yield return item;
            }
            if (node.Y > 0 && positions.Contains(new Vector(node.X, node.Y - 1)))
            {
                if (!visited.Contains(new Vector(node.X, node.Y - 1)))
                    foreach (var item in GetGroupRecursive(positions, visited, new Vector(node.X, node.Y - 1)))
                        yield return item;
            }
            if (node.Y < Height - 1 && positions.Contains(new Vector(node.X, node.Y + 1)))
            {
                if (!visited.Contains(new Vector(node.X, node.Y + 1)))
                    foreach (var item in GetGroupRecursive(positions, visited, new Vector(node.X, node.Y + 1)))
                        yield return item;
            }
        }
    }

    public record Vector(int X, int Y)
    {
        public static Vector operator +(Vector left, Vector right) => new Vector(left.X + right.X, left.Y + right.Y);
        public static Vector operator -(Vector left, Vector right) => new Vector(left.X - right.X, left.Y - right.Y);
    }

    private class Robot(Vector position, Vector velocity)
    {
        public Vector Position { get; set; } = position;
        public Vector Velocity { get; set; } = velocity;
    }

    public class ChristmasTreeNotFoundException : Exception
    { }
}
