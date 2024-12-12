namespace AoC24.Solutions.Day12;

public partial class Part1(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\1201.txt";
    private readonly string _filePath = filePath;

    public Part1()
        : this(FilePath)
    { }

    public long GetResult() => Method1();

    public long Method1()
    {
        var nodeLookup = File.ReadLines(_filePath)
            .SelectMany((line, y) => line.Select((c, x) => new GridTile(c, new Vector(x, y))).ToArray())
            .ToDictionary((item) => item.Position, (item) => item);

        var getAdjacent = new Func<Dictionary<Vector, GridTile>, GridTile, GridTile[]>((grid, input) =>
        {
            var results = new List<GridTile>();
            if (grid.TryGetValue(input.Position.Left, out var left) && left.Name == input.Name)
                results.Add(left);
            if (grid.TryGetValue(input.Position.Right, out var right) && right.Name == input.Name)
                results.Add(right);
            if (grid.TryGetValue(input.Position.Top, out var top) && top.Name == input.Name)
                results.Add(top);
            if (grid.TryGetValue(input.Position.Bottom, out var bottom) && bottom.Name == input.Name)
                results.Add(bottom);

            return [.. results];
        });

        // Build adjacency lists.
        nodeLookup.Values.ToList().ForEach((node) => node.Adjacent = new List<GridTile>(getAdjacent(nodeLookup, node)));

        // Get edges. If we process nodes left-to-right, top-to-bottom, we can look 'forward' (to the right and bottom) to get all unique edges without duplicates, including a direction of the edge.
        nodeLookup.Values.ToList().ForEach((node) => node.Edges = node.Adjacent.Where((adj) => adj.Position > node.Position).Select((adj) => new Edge(node.Position, adj.Position)).ToList());

        // Now that we have unique edges, we also have exactly 1 node in each group that has no (outgoing) edges. These are the HEADs.
        var heads = nodeLookup.Values.Where((node) => node.Edges.Count() == 0).ToList();

        // Create groups which contain: the number of boundaries (for perimeter calculation) and a hashset of all positions that belong to this group.
        // Area is defined by the number of nodes, perimeter by the inverse of the count of adjacent nodes.
        // Note: I made a tiny mistake in my reasoning, there can be more than one node in an area with no 'outgoing' edges.
        // For example, this one has 3 nodes that have no edges to the right/bottom:
        /*
         * ....OOO....
         * ....OOOO...
         * ....OOO....
         * ....OOO....
         * .....O.....
         */
        // Maybe fix it properly for part 2... For now, workaround is to deduplicate now while building the groups.
        var visited = new HashSet<Vector>();
        var groups = new List<HashSet<(char name, Vector position, int boundaries)>>();
        foreach (var node in heads)
        {
            // We must iterate here, otherwise it will return only the input node.
            var connectedNodes = GetConnectedNodes(node, new()).ToArray();
            if (connectedNodes.Any((item) => visited.Contains(item.Position)))
            {
                // Already part of other group
                continue;
            }
            groups.Add(new HashSet<(char, Vector, int)>(connectedNodes.Select((item) => (item.Name, item.Position, 4 - item.Adjacent.Count))));
            visited.Add(node.Position);
        }

        var result = groups.Sum((item) =>
        {
            var area = item.Count();
            var perimeter = item.Sum((node) => node.boundaries);

            return area * perimeter;
        });

        return result;
    }

    public long Method2()
    {
        // TODO: Apply improvement from part 2
        return Method1();
    }

    private static IEnumerable<GridTile> GetConnectedNodes(GridTile input, HashSet<Vector> visited)
    {
        visited.Add(input.Position);
        yield return input;
        foreach (var adjacent in input.Adjacent.Where((item) => !visited.Contains(item.Position)))
        {
            visited.Add(adjacent.Position);
            yield return adjacent;

            foreach (var next in GetConnectedNodes(adjacent, visited))
            {
                yield return next;
            }
        }
    }

    public record Vector(int X, int Y) : IComparable<Vector>
    {
        public static implicit operator Vector((int, int) valueTuple) => new Vector(valueTuple.Item1, valueTuple.Item2);
        public static implicit operator (int, int)(Vector vector) => (vector.X, vector.Y);

        public static bool operator <(Vector left, Vector right) => left.CompareTo(right) < 0;
        public static bool operator >(Vector left, Vector right) => left.CompareTo(right) > 0;

        public Vector Left => new(X - 1, Y);
        public Vector Right => new(X + 1, Y);
        public Vector Top => new(X, Y - 1);
        public Vector Bottom => new(X, Y + 1);

        public int CompareTo(Vector? other)
        {
            if (X < other?.X || Y < other?.Y) return -1;
            else if (X > other?.X || Y > other?.Y) return 1;
            else return 0;
        }

        public override string ToString() => $"({X}, {Y})";
    }

    public record Edge(Vector From, Vector To)
    {
        public override string ToString() => $"{From} -> {To}";
    }

    public class GridTile
    {
        public char Name { get; }
        public Vector Position { get; }
        public List<GridTile> Adjacent { get; set; } = new List<GridTile>();
        public List<Edge> Edges { get; set; } = new List<Edge>();
        public int BoundaryCount { get; set; }

        public GridTile(char name, Vector position)
        {
            Name = name;
            Position = position;
        }

        public override string ToString() => $"{Name} {Position}";
    }
}
