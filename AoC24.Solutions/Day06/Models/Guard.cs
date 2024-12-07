namespace AoC24.Solutions.Day06.Models;

public record Guard
{
    public static readonly char[] GuardSymbols = ['^', '>', 'v', '<'];
    public static readonly Dictionary<char, Vector> GuardOrientations = new()
        {
            { '^', Vector.North },
            { '>', Vector.East },
            { 'v', Vector.South },
            { '<', Vector.West },
        };

    private readonly HashSet<(Point position, Vector orientation)> _visited;

    public Point Position { get; private set; }
    public Vector Orientation { get; private set; }

    public Guard(char symbol, int x, int y)
        : this(new Point(x, y), GuardOrientations[symbol])
    { }

    public Guard(Point position, Vector orientation)
    {
        Position = position;
        Orientation = orientation;

        _visited = new HashSet<(Point position, Vector orientation)>();
    }

    public IEnumerable<(Tile Tile, Vector Orientation)> GetRouteUntilOut(Laboratory lab, bool addToVisited = true)
    {
        var grid = lab.Grid;
        // Also return the starting position
        yield return (grid[Position.Y][Position.X], Orientation);

        while (lab.IsInBounds(Position))
        {
            // Turn right until we find the next free space.
            while (
                Position + Orientation is Point nextPosition
                && lab.IsInBounds(nextPosition)
                && grid[nextPosition.Y][nextPosition.X].TileType == TileType.Obstacle
                )
            {
                Orientation = Vector.TurnClockwise(Orientation);
            }

            Position += Orientation;

            if (lab.IsInBounds(Position))
            {
                yield return (grid[Position.Y][Position.X], Orientation);
                if (addToVisited)
                {
                    _visited.Add((Position, Orientation));
                }
            }
        }
    }

    public bool WillGetStuckInLoop(Laboratory lab)
    {
        var localVisitedCache = new HashSet<(Point position, Vector orientation)>();
        foreach (var item in GetRouteUntilOut(lab, false))
        {
            if (localVisitedCache.Contains((item.Tile.Position, item.Orientation)))
                return true;

            localVisitedCache.Add((item.Tile.Position, item.Orientation));
        }

        return false;
    }

    public void ResetTo(Point position, Vector orientation)
    {
        Position = position;
        Orientation = orientation;
    }
}
