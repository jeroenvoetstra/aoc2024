namespace AoC24.Solutions.Day15;

public partial class Part1(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\1501.txt";
    private readonly string _filePath = filePath;

    public Part1()
        : this(FilePath)
    { }

    public long GetResult()
    {
        var content = File.ReadAllText(_filePath);
        var parts = content.Split($"{Environment.NewLine}{Environment.NewLine}");

        var grid = parts[0].Split(Environment.NewLine)
            .Select((line, y) =>
                line.Select((c, x) => new
                    {
                        Position = new Vector(x, y),
                        Type = c switch { '#' => TileType.Wall, '@' => TileType.Robot, '.' => TileType.FreeSpace, 'O' => TileType.Box, _ => throw new InvalidOperationException() },
                    })
                    .ToArray()
                ).ToArray();
        var inputs = parts[1]
            .Split(Environment.NewLine)
            .SelectMany((line) =>
                line.Select((c) => c switch { '<' => Vector.Left, '>' => Vector.Right, '^' => Vector.Up, 'v' => Vector.Down, _ => throw new InvalidOperationException() })
                    .ToArray()
                ).ToArray();

        var robotPosition = grid.SelectMany((row) => row.Where((tile) => tile.Type == TileType.Robot)).Select((tile) => tile.Position).Single();
        var boxPositions = new HashSet<Vector>(grid.SelectMany((row) => row.Where((tile) => tile.Type == TileType.Box)).Select((tile) => tile.Position));
        var wallPositions = new HashSet<Vector>(grid.SelectMany((row) => row.Where((tile) => tile.Type == TileType.Wall)).Select((tile) => tile.Position));

        foreach (var input in inputs)
        {
            var next = robotPosition + input;
            if (wallPositions.Contains(next))
            {
                // do nothing
                continue;
            }
            else if (boxPositions.Contains(next))
            {
                PushBoxes(ref boxPositions, wallPositions, next, input);
                
                // If the next still contains a box, they couldn't be pushed any further
                if (boxPositions.Contains(next))
                {
                    // do nothing
                    continue;
                }
            }

            robotPosition = next;
        }

        return boxPositions.Sum((item) => 100 * item.Y + item.X);
    }

    private static bool PushBoxes(ref HashSet<Vector> boxPositions, HashSet<Vector> wallPositions, Vector from, Vector direction)
    {
        var next = from + direction;
        if (!wallPositions.Contains(next))
        {
            if (!boxPositions.Contains(next) || PushBoxes(ref boxPositions, wallPositions, next, direction))
            {
                boxPositions.Remove(from);
                boxPositions.Add(next);

                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    public enum TileType
    {
        FreeSpace,
        Wall,
        Robot,
        Box,
    }

    public record Vector(int X, int Y)
    {
        public static readonly Vector Up = new(0, -1);
        public static readonly Vector Down = new(0, 1);
        public static readonly Vector Left = new(-1, 0);
        public static readonly Vector Right = new(1, 0);

        public static Vector operator +(Vector left, Vector right) => new(left.X + right.X, left.Y + right.Y);
        public static Vector operator -(Vector left, Vector right) => new(left.X - right.X, left.Y - right.Y);
        public static Vector operator *(int left, Vector right) => new(left * right.X, left * right.Y);
    }
}
