namespace AoC24.Solutions.Day08.Models;

public record Vector(int X, int Y) : Point(X, Y)
{
    public static Point operator *(int multiplier, Vector direction) => new(multiplier * direction.X, multiplier * direction.Y);
}
