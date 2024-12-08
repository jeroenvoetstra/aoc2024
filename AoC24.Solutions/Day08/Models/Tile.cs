namespace AoC24.Solutions.Day08.Models;

public record Tile(Point Position)
{
    public static Tile Create(char type, Point position)
    {
        return (int)type switch
        {
            '.' => new Tile(position),
            >= 48 and <= 57 => new AntennaTile(type, position), // 0-9
            >= 65 and <= 90 => new AntennaTile(type, position), // A-Z
            >= 97 and <= 122 => new AntennaTile(type, position), // a-z
            _ => throw new ArgumentException(nameof(type)),
        };
    }
}
