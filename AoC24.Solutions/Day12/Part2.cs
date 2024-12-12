using static AoC24.Solutions.Day12.Part2;

namespace AoC24.Solutions.Day12;

public partial class Part2(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\1201.txt";
    private readonly string _filePath = filePath;

    public Part2()
        : this(FilePath)
    { }

    public long GetResult()
    {
        var grid = File.ReadLines(_filePath)
            .Select((line, y) => line.Select((c, x) => new Node(c, new Vector(x, y))).ToArray())
            .ToArray();

        var width = grid[0].Length;
        var height = grid.Length;

        var visited = Enumerable.Range(0, height).Select((row) => Enumerable.Range(0, width).Select((_) => false).ToArray()).ToArray();
        var groups = grid
            // Get all individual nodes.
            .SelectMany((row) => row.Select((col) => col))
            // Recursive DFS with visited cache for creation of groups (improvement over part 1).
            .Select((node) => GetGroupRecursive(grid, visited, node).ToArray())
            // Select only the groups that contain nodes.
            .Where((arr) => arr.Length > 0)
            // Final processing.
            .Select((group) => new
            {
                // FOR DEBUGGING
                /*group.First().Name,*/
                // FOR DEBUGGING
                /*Nodes = group.Select((node) => node).ToArray(),*/
                // The count of positions will equal the total area of the plot
                Positions = group.Select((node) => node.Position).ToArray(),
                // The count of corners (90° or 270°) present in the plot will equal the number of sides
                Corners = group.Sum((node) => node.CornerCount)
            })
            .ToArray()
            ;

        // Multiply the sides by the area.
        return groups.Sum((group) => group.Corners * group.Positions.Length);
    }

    private static IEnumerable<Node> GetGroupRecursive(Node[][] grid, bool[][] visited, Node node)
    {
        if (!visited[node.Position.Y][node.Position.X])
        {
            visited[node.Position.Y][node.Position.X] = true;
            yield return node;

            // Check to the left of the current node.
            if (node.Position.X > 0 && node.Name == grid[node.Position.Y][node.Position.X - 1].Name)
            {
                node.Adjacent.Add(grid[node.Position.Y][node.Position.X - 1]);
                if (!visited[node.Position.Y][node.Position.X - 1])
                    foreach (var item in GetGroupRecursive(grid, visited, grid[node.Position.Y][node.Position.X - 1]))
                        yield return item;
            }
            // Check to the right of the current node.
            if (node.Position.X < (grid[node.Position.Y].Length - 1) && node.Name == grid[node.Position.Y][node.Position.X + 1].Name)
            {
                node.Adjacent.Add(grid[node.Position.Y][node.Position.X + 1]);
                if (!visited[node.Position.Y][node.Position.X + 1])
                    foreach (var item in GetGroupRecursive(grid, visited, grid[node.Position.Y][node.Position.X + 1]))
                        yield return item;
            }
            // Check above the current node.
            if (node.Position.Y > 0 && node.Name == grid[node.Position.Y - 1][node.Position.X].Name)
            {
                node.Adjacent.Add(grid[node.Position.Y - 1][node.Position.X]);
                if (!visited[node.Position.Y - 1][node.Position.X])
                    foreach (var item in GetGroupRecursive(grid, visited, grid[node.Position.Y - 1][node.Position.X]))
                        yield return item;
            }
            // Check below the current node.
            if (node.Position.Y < (grid.Length - 1) && node.Name == grid[node.Position.Y + 1][node.Position.X].Name)
            {
                node.Adjacent.Add(grid[node.Position.Y + 1][node.Position.X]);
                if (!visited[node.Position.Y + 1][node.Position.X])
                    foreach (var item in GetGroupRecursive(grid, visited, grid[node.Position.Y + 1][node.Position.X]))
                        yield return item;
            }
        }
    }

    public record Vector(int X, int Y);

    public class Node(char name, Vector position)
    {
        private static readonly Func<Node, Node, bool> IsNorth = new((source, node) => node.Position.X == source.Position.X && node.Position.Y == source.Position.Y - 1);
        private static readonly Func<Node, Node, bool> IsEast = new((source, node) => node.Position.X == source.Position.X + 1 && node.Position.Y == source.Position.Y);
        private static readonly Func<Node, Node, bool> IsSouth = new((source, node) => node.Position.X == source.Position.X && node.Position.Y == source.Position.Y + 1);
        private static readonly Func<Node, Node, bool> IsWest = new((source, node) => node.Position.X == source.Position.X - 1 && node.Position.Y == source.Position.Y);

        public char Name { get; } = name;
        public Vector Position { get; } = position;
        public List<Node> Adjacent { get; } = [];

        // Corner logic, is used to calculate number of sides of a combination of nodes.

        public bool HasNorthBoundary => Adjacent.All((node) => !IsNorth(this, node));
        public bool HasEastBoundary => Adjacent.All((node) => !IsEast(this, node));
        public bool HasSouthBoundary => Adjacent.All((node) => !IsSouth(this, node));
        public bool HasWestBoundary => Adjacent.All((node) => !IsWest(this, node));

        public bool HasNorthAdjacent => !HasNorthBoundary;
        public bool HasEastAdjacent => !HasEastBoundary;
        public bool HasSouthAdjacent => !HasSouthBoundary;
        public bool HasWestAdjacent => !HasWestBoundary;

        public Node? NorthAdjacent => HasNorthAdjacent ? Adjacent.First((node) => IsNorth(this, node)) : null;
        public Node? EastAdjacent => HasEastAdjacent ? Adjacent.First((node) => IsEast(this, node)) : null;
        public Node? SouthAdjacent => HasSouthAdjacent ? Adjacent.First((node) => IsSouth(this, node)) : null;
        public Node? WestAdjacent => HasWestAdjacent ? Adjacent.First((node) => IsWest(this, node)) : null;

        public bool IsNorthEastInsideCorner => HasNorthBoundary && HasEastBoundary;
        public bool IsSouthEastInsideCorner => HasEastBoundary && HasSouthBoundary;
        public bool IsSouthWestInsideCorner => HasSouthBoundary && HasWestBoundary;
        public bool IsNorthWestInsideCorner => HasNorthBoundary && HasWestBoundary;

        public bool IsNorthEastOutsideCorner => HasNorthAdjacent && HasEastAdjacent && NorthAdjacent!.HasEastBoundary && EastAdjacent!.HasNorthBoundary;
        public bool IsSouthEastOutsideCorner => HasEastAdjacent && HasSouthAdjacent && EastAdjacent!.HasSouthBoundary && SouthAdjacent!.HasEastBoundary;
        public bool IsSouthWestOutsideCorner => HasSouthAdjacent && HasWestAdjacent && SouthAdjacent!.HasWestBoundary && WestAdjacent!.HasSouthBoundary;
        public bool IsNorthWestOutsideCorner => HasWestAdjacent && HasNorthAdjacent && WestAdjacent!.HasNorthBoundary && NorthAdjacent!.HasWestBoundary;

        public int CornerCount => (new bool[]
            {
                IsNorthEastInsideCorner, IsSouthEastInsideCorner, IsSouthWestInsideCorner, IsNorthWestInsideCorner,
                IsNorthEastOutsideCorner, IsSouthEastOutsideCorner, IsSouthWestOutsideCorner, IsNorthWestOutsideCorner
            })
            .Where((b) => b == true)
            .Count()
            ;

        //

        // FOR DEBUGGING
        //public override string ToString() => $"{Name} {Position}";
    }
}
