namespace AoC24.Solutions.Day16;

public partial class Part1(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\1601.txt";
    private readonly string _filePath = filePath;

    public Part1()
        : this(FilePath)
    { }

    public long GetResult()
    {
        var content = File.ReadAllLines(_filePath);

        return FindScoreUsingAStar(content);
    }

    public static long FindScoreUsingAStar(string[] content)
    {
        var validPathIndicators = new char[] { '.', 'S', 'E' };
        var mazePathItems = content
            .Select((line, y) => line.Select((c, x) => validPathIndicators.Contains(c) ? new { Position = new Vector(x, y), Type = c } : null))
            // Flatten list
            .SelectMany((item) => item)
            // Filter out nulls
            .Where((item) => item != null).Select((item) => item!)
            .ToArray();

        var start = mazePathItems.Single((item) => item.Type == 'S').Position;
        var target = mazePathItems.Single((item) => item.Type == 'E').Position;
        var maze = new HashSet<Vector>(mazePathItems.Select((item) => item.Position));

        var visited = new HashSet<Vector>();

        var score = AStarSearch(
            maze.ToDictionary((item) => item, (item) => new Node() { Position = item }),
            target,
            start
            )?.ToArray();

        return score?.FirstOrDefault()?.G ?? 0;
    }

    public static long Method2(string[] content)
    {
        var validPathIndicators = new char[] { '.', 'S', 'E' };
        var mazePathItems = content
            .Select((line, y) => line.Select((c, x) => validPathIndicators.Contains(c) ? new { Position = new Vector(x, y), Type = c } : null))
            // Flatten list
            .SelectMany((item) => item)
            // Filter out nulls
            .Where((item) => item != null).Select((item) => item!)
            .ToArray();

        var start = mazePathItems.Single((item) => item.Type == 'S').Position;
        var target = mazePathItems.Single((item) => item.Type == 'E').Position;
        var maze = new HashSet<Vector>(mazePathItems.Select((item) => item.Position));

        var visited = new HashSet<Vector>();

        var scores = ScorePaths(
            maze.ToDictionary((item) => item, (item) => new Node() { Position = item }),
            target,
            start
            )?.ToArray();

        return scores?.Min() ?? 0;
    }

    private static List<Node>? AStarSearch(Dictionary<Vector, Node> maze, Vector target, Vector start)
    {
        var openList = new List<Node>([maze[start]]);
        var closedList = new List<Node>();
        while (openList.Count > 0)
        {
            var currentNode = openList.OrderBy((item) => item.F).First();
            openList.Remove(currentNode);

            var adjacents = (new Vector[] { currentNode.Position + Vector.Right, currentNode.Position + Vector.Down, currentNode.Position + Vector.Left, currentNode.Position + Vector.Up, })
                .Where(maze.ContainsKey)
                .ToArray()
                ;
            foreach (var successor in adjacents)
            {
                var adjacentNode = maze[successor];

                // There is a bug here that prevents converging paths from properly selecting the lowest score one (if the
                // point of convergence causes a switch to a cheaper path, this path is now blocked by the convergence
                // node being in the closed list). For example:
                /*
                 * ##########
                 * #y..x...E#
                 * #.##.#####
                 * #..#.....#
                 * ##.#####.#
                 * #S.......#
                 * ##########
                 */
                // When the node marked with [x] is processed (from below), at that point the current path's cost will become
                // greater than the path waiting at the node [y]. When [y] starts walking towards the end, it encounters [x]
                // and cancels out because [x] is already processed. So it hops back to the more expensive path (4019 instead
                // of 4013).
                if (closedList.Contains(adjacentNode))
                    continue;

                adjacentNode.Parent = currentNode;
                adjacentNode.Direction = adjacentNode.Position - currentNode.Position;

                adjacentNode.G = currentNode.G + (currentNode.Position + currentNode.Direction != successor ? 1001 : 1);
                adjacentNode.H = (target - successor).AsManhattanDistance;
                adjacentNode.F = adjacentNode.G + adjacentNode.H;

                if (successor == target)
                {
                    var result = new List<Node>()
                    {
                        adjacentNode,
                        currentNode
                    };
                    var item = currentNode.Parent;
                    while (item != null)
                    {
                        result.Add(item);
                        item = item.Parent;
                    }
                    return result;
                }

                if (openList.Any((item) => item.Position == successor && item.F < adjacentNode.F))
                    continue;

                if (closedList.Any((item) => item.Position == successor && item.F < adjacentNode.F))
                    continue;

                openList.Add(adjacentNode);
            }

            closedList.Add(currentNode);
        }

        return null;
    }

    private static IEnumerable<int> ScorePaths(Dictionary<Vector, Node> maze, Vector target, Vector start)
    {
        var visited = new HashSet<Vector>();
        var queue = new Queue<Vector>([start]);
        while (queue.Count > 0)
        {
            var currentPosition = queue.Dequeue();
            var currentNode = maze[currentPosition];

            visited.Add(currentPosition);

            var adjacents = (new Vector[] { currentNode.Position + Vector.Right, currentNode.Position + Vector.Down, currentNode.Position + Vector.Left, currentNode.Position + Vector.Up, })
                .Where(maze.ContainsKey)
                .ToArray()
                ;

            foreach (var adjacentPosition in adjacents)
            {
                if (visited.Contains(adjacentPosition))
                    continue;

                var adjacentNode = maze[adjacentPosition];
                adjacentNode.Parent = currentNode;
                adjacentNode.Direction = adjacentNode.Position - currentPosition;

                var adjacentNodeG = currentNode.Position + currentNode.Direction != adjacentPosition ? 1001 : 1;

                if (adjacentPosition == target)
                {
                    yield return currentNode.G + adjacentNodeG;
                    continue;
                }

                adjacentNode.G = currentNode.G + adjacentNodeG;

                queue.Enqueue(adjacentPosition);
            }
        }
    }
}

public record Vector(int X, int Y)
{
    public static readonly Vector Up = new(0, -1);
    public static readonly Vector Down = new(0, 1);
    public static readonly Vector Left = new(-1, 0);
    public static readonly Vector Right = new(1, 0);

    public Vector Absolute => new(Math.Abs(X), Math.Abs(Y));

    // Heuristics
    public int AsDistance => (int)(Math.Pow(X, 2) + Math.Pow(Y, 2));
    public int AsManhattanDistance => Math.Abs(X) + Math.Abs(Y);
    public int AsDiagonalDistance => Math.Max(Math.Abs(X), Math.Abs(Y));

    public static Vector operator +(Vector left, Vector right) => new(left.X + right.X, left.Y + right.Y);
    public static Vector operator -(Vector left, Vector right) => new(left.X - right.X, left.Y - right.Y);
    public static Vector operator *(int left, Vector right) => new(left * right.X, left * right.Y);

    public override string ToString() => $"({X}, {Y})";
}

public class Node
{
    public required Vector Position { get; set; }
    public Node? Parent { get; set; }
    public int F { get; set; }
    public int G { get; set; }
    public int H { get; set; }
    public Vector Direction { get; set; } = Vector.Right;

    public override string ToString() => $"{Position}, F={F}, G={G}, H={H}";
}

