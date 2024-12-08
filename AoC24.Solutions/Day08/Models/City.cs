namespace AoC24.Solutions.Day08.Models;

public record City
{
    private readonly Bounds _bounds;

    public Tile[][] Grid { get; }

#if DEBUG
    public string VisualizedGrid =>
        string.Join(
            "\n",
            Grid.Select((row) =>
                new string(
                    row.Select((tile) => tile switch
                    {
                        AntennaTile antenna => antenna.Name,
                        Tile _ => '.',
                        _ => throw new InvalidOperationException()
                    }).ToArray()
                    )
                )
            );
#endif

    public City(string[] input)
    {
        Grid = input.Select((line, y) => line.Select((tile, x) => Tile.Create(tile, new Point(x, y))).ToArray()).ToArray();

        _bounds = new Bounds() { Top = 0, Left = 0, Bottom = Grid.Length, Right = Grid[0].Length };
    }

    public bool IsInBounds(Point position) => IsInBounds(position.X, position.Y);
    public bool IsInBounds(int x, int y) => x >= _bounds.Left && x < _bounds.Right && y >= _bounds.Top && y < _bounds.Bottom;

    private record Bounds
    {
        public int Top { get; init; }
        public int Right { get; init; }
        public int Bottom { get; init; }
        public int Left { get; init; }
    }
}
