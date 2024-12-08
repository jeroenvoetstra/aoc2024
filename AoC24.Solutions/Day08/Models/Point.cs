namespace AoC24.Solutions.Day08.Models;

public record Point(int X, int Y)
{
    public static Vector operator +(Point point, Point direction) => new(point.X + direction.X, point.Y + direction.Y);
    public static Vector operator -(Point point, Point direction) => new(point.X - direction.X, point.Y - direction.Y);
    public static Point operator +(Point point, Vector direction) => new(point.X + direction.X, point.Y + direction.Y);
    public static Point operator -(Point point, Vector direction) => new(point.X - direction.X, point.Y - direction.Y);
}
