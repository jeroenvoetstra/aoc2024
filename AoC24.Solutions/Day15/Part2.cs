namespace AoC24.Solutions.Day15;

public partial class Part2(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\1501.txt";
    private readonly string _filePath = filePath;

    public Part2()
        : this(FilePath)
    { }

    public long GetResult()
    {
        var content = File.ReadAllText(_filePath);
        var parts = content.Split($"{Environment.NewLine}{Environment.NewLine}");

        var grid = parts[0].Split(Environment.NewLine)
            .Select((line, y) =>
                line.SelectMany((c, x) => new[]
                    {
                        new
                        {
                            Position = new Vector(x * 2, y * 1),
                            Type = c switch { '#' => TileType.Wall, '@' => TileType.Robot, '.' => TileType.FreeSpace, 'O' => TileType.Box, _ => throw new InvalidOperationException() },
                        },
                        new
                        {
                            Position = new Vector(x * 2 + 1, y * 1),
                            Type = c switch { '#' => TileType.Wall, '@' => TileType.FreeSpace, '.' => TileType.FreeSpace, 'O' => TileType.FreeSpace, _ => throw new InvalidOperationException() },
                        }
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
        var boxLeftPartPositions = new HashSet<Vector>(grid.SelectMany((row) => row.Where((tile) => tile.Type == TileType.Box)).Select((tile) => tile.Position));
        var boxRightPartPositions = new HashSet<Vector>(grid.SelectMany((row) => row.Where((tile) => tile.Type == TileType.Box)).Select((tile) => tile.Position + Vector.Right));
        var wallPositions = new HashSet<Vector>(grid.SelectMany((row) => row.Where((tile) => tile.Type == TileType.Wall)).Select((tile) => tile.Position));

        var counter = 0;
        foreach (var input in inputs)
        {
            counter++;
            var next = robotPosition + input;
            if (wallPositions.Contains(next))
            {
                // do nothing
                continue;
            }

            var success = false;
            if (input.Y != 0) // up/down
            {
                if (boxLeftPartPositions.Contains(next))
                {
                    success = PushBoxesUpDown(ref boxLeftPartPositions, ref boxRightPartPositions, wallPositions, (next, next + Vector.Right), input);
                }
                else if (boxRightPartPositions.Contains(next))
                {
                    success = PushBoxesUpDown(ref boxLeftPartPositions, ref boxRightPartPositions, wallPositions, (next + Vector.Left, next), input);
                }
                else
                {
                    success = true;
                }
            }
            else if (input == Vector.Left)
            {
                if (boxRightPartPositions.Contains(next))
                {
                    success = PushBoxesLeft(ref boxLeftPartPositions, ref boxRightPartPositions, wallPositions, (next + Vector.Left, next), input);
                }
                else
                {
                    success = true;
                }
            }
            else if (input == Vector.Right)
            {
                if (boxLeftPartPositions.Contains(next))
                {
                    success = PushBoxesRight(ref boxLeftPartPositions, ref boxRightPartPositions, wallPositions, (next, next + Vector.Right), input);
                }
                else
                {
                    success = true;
                }
            }

            if (success)
            {
                robotPosition = next;
            }
        }

        return boxLeftPartPositions.Sum((item) => 100 * item.Y + item.X);
    }

    private static bool PushBoxesUpDown(ref HashSet<Vector> boxLeftPositions, ref HashSet<Vector> boxRightPositions, HashSet<Vector> wallPositions, (Vector, Vector) from, Vector direction)
    {
        var next = (from.Item1 + direction, from.Item2 + direction);
        if (!wallPositions.Contains(next.Item1) && !wallPositions.Contains(next.Item2))
        {
            var success = false;
            if (boxLeftPositions.Contains(next.Item1)) // straight above
            {
                success = PushBoxesUpDown(ref boxLeftPositions, ref boxRightPositions, wallPositions, (next.Item1, next.Item2), direction);
            }
            else if (boxRightPositions.Contains(next.Item1) && boxLeftPositions.Contains(next.Item2)) // two above or below that both need to be pushed
            {
                // First do a dry run to see if it's possible to move both. If not, we don't have to revert, otherwise we accept that we have to execute the logic twice.
                var copyLeft = new HashSet<Vector>(boxLeftPositions);
                var copyRight = new HashSet<Vector>(boxRightPositions);
                var successLeft = PushBoxesUpDown(ref copyLeft, ref copyRight, wallPositions, (next.Item1 + Vector.Left, next.Item1), direction);
                var successRight = PushBoxesUpDown(ref copyLeft, ref copyRight, wallPositions, (next.Item2, next.Item2 + Vector.Right), direction);
                if (successLeft && successRight)
                {
                    success =
                        PushBoxesUpDown(ref boxLeftPositions, ref boxRightPositions, wallPositions, (next.Item1 + Vector.Left, next.Item1), direction)
                            && PushBoxesUpDown(ref boxLeftPositions, ref boxRightPositions, wallPositions, (next.Item2, next.Item2 + Vector.Right), direction);
                }
            }
            else if (boxLeftPositions.Contains(next.Item2)) // one above or below, on the right side
            {
                success = PushBoxesUpDown(ref boxLeftPositions, ref boxRightPositions, wallPositions, (next.Item2, next.Item2 + Vector.Right), direction);
            }
            else if (boxRightPositions.Contains(next.Item1)) // one above or below, on the left side
            {
                success = PushBoxesUpDown(ref boxLeftPositions, ref boxRightPositions, wallPositions, (next.Item1 + Vector.Left, next.Item1), direction);
            }
            else
            {
                // free to move
                success = true;
            }

            if (success)
            {
                boxLeftPositions.Remove(from.Item1);
                boxRightPositions.Remove(from.Item2);
                boxLeftPositions.Add(next.Item1);
                boxRightPositions.Add(next.Item2);
            }

            return success;
        }
        else
        {
            return false;
        }
    }

    private static bool PushBoxesLeft(ref HashSet<Vector> boxLeftPositions, ref HashSet<Vector> boxRightPositions, HashSet<Vector> wallPositions, (Vector, Vector) first, Vector direction)
    {
        var next = first.Item1 + direction;
        if (!wallPositions.Contains(next))
        {
            if (!boxRightPositions.Contains(next) || PushBoxesLeft(ref boxLeftPositions, ref boxRightPositions, wallPositions, (next + Vector.Left, next), direction))
            {
                boxLeftPositions.Remove(first.Item1);
                boxRightPositions.Remove(first.Item2);
                boxLeftPositions.Add(next);
                boxRightPositions.Add(next + Vector.Right);

                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    private static bool PushBoxesRight(ref HashSet<Vector> boxLeftPositions, ref HashSet<Vector> boxRightPositions, HashSet<Vector> wallPositions, (Vector, Vector) first, Vector direction)
    {
        var next = first.Item2 + direction;
        if (!wallPositions.Contains(next))
        {
            if (!boxLeftPositions.Contains(next) || PushBoxesRight(ref boxLeftPositions, ref boxRightPositions, wallPositions, (next, next + Vector.Right), direction))
            {
                boxLeftPositions.Remove(first.Item1);
                boxRightPositions.Remove(first.Item2);
                boxLeftPositions.Add(next + Vector.Left);
                boxRightPositions.Add(next);

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
