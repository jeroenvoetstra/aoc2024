namespace AoC24.Solutions.Day06.Models;

public record Tile
{
    public Point Position { get; private set; }
    public TileType TileType { get; private set; }

    public Tile(char type, int x, int y)
    {
        TileType = type switch
        {
            '.' or '^' or '>' or 'v' or '<' => TileType.FreeSpace,
            '#' => TileType.Obstacle,
            _ => throw new ArgumentException(nameof(type)),
        };
        Position = new Point(x, y);
    }

    public void ChangeType(TileType type)
    {
        TileType = type;
    }
}
