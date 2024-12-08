namespace AoC24.Solutions.Day06.Models;

public class Laboratory
{
    private readonly Bounds _bounds;
    private readonly HashSet<string> _obstacleIndex;

    public Tile[][] Grid { get; }

#if DEBUG
    public string VisualizedGrid =>
        string.Join(
            "\n",
            Grid.Select((row) =>
                new string(
                    row.Select((tile) => tile.TileType switch
                    {
                        TileType.FreeSpace => '.',
                        TileType.Obstacle => '#',
                        _ => throw new InvalidOperationException()
                    }).ToArray()
                    )
                )
            );
#endif

    public Laboratory(Tile[][] tiles)
    {
        if (tiles == null || tiles.Length == 0 || tiles[0] == null || tiles[0].Length == 0)
            throw new ArgumentException(nameof(tiles));

        Grid = tiles;
        _bounds = new Bounds() { Top = 0, Left = 0, Bottom = Grid.Length, Right = Grid[0].Length };
        _obstacleIndex = new HashSet<string>(
            tiles
                .SelectMany((line) => line)
                .Where((tile) => tile.TileType == TileType.Obstacle)
                .Select((tile) => $"{tile.Position.X},{tile.Position.Y}")
            );
    }

    public bool IsInBounds(Point position) => IsInBounds(position.X, position.Y);
    public bool IsInBounds(int x, int y) => x >= _bounds.Left && x < _bounds.Right && y >= _bounds.Top && y < _bounds.Bottom;

    public bool HasObstacleInDirection(Point position, Vector direction)
    {
        if (!IsInBounds(position))
            return false;

        for (var currentPosition = position; IsInBounds(currentPosition); currentPosition += direction)
        {
            //if (Grid[currentPosition.Y][currentPosition.X].TileType == TileType.Obstacle)
            //    return true;
            if (_obstacleIndex.Contains($"{currentPosition.X},{currentPosition.Y}"))
                return true;
        }

        return false;
    }

    private record Bounds
    {
        public int Top { get; init; }
        public int Right { get; init; }
        public int Bottom { get; init; }
        public int Left { get; init; }
    }
}
