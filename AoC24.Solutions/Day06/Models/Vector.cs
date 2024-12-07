namespace AoC24.Solutions.Day06.Models;

public record Vector(int X, int Y) : Point(X, Y)
{
    public static readonly Vector North = new(0, -1);
    public static readonly Vector East = new(1, 0);
    public static readonly Vector South = new(0, 1);
    public static readonly Vector West = new(-1, 0);

    public static Vector TurnClockwise(Vector start)
    {
        if (start == North)
            return East;
        else if (start == East)
            return South;
        else if (start == South)
            return West;
        else if (start == West)
            return North;
        else
            throw new InvalidOperationException();
    }
}
