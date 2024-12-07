namespace AoC24.Solutions.Day06.Models;

public record Point(int X, int Y)
{
    public static Point operator +(Point point, Vector direction) => new(point.X + direction.X, point.Y + direction.Y);
    public static Point operator -(Point point, Vector direction) => new(point.X - direction.X, point.Y - direction.Y);
}
