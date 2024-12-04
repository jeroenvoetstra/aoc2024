namespace AoC24.Solutions.Day04;

public partial class Part1 : IAoCSolution
{
    static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0401.txt";

    public long GetResult()
    {
        var characterMatrix = File.ReadAllLines(FilePath).ToArray();
        var characterGrid = characterMatrix.SelectMany((line, y) => line.Select((c, x) => new CharPoint(c, new Vector(x, y)))).ToArray();
        // Build predicate chain from back to front (X predicate will depend on the M one, M on A and so on.
        var sPredicate = new Func<CharPoint, CharPoint, CharPoint, CharPoint, bool>((x, m, a, s) =>
            s.Character == 'S'
                && Distance(a, s) == 1 && Distance(m, s) == 2 && Distance(x, s) == 3
                && s.GetDirection(a) == a.GetDirection(m)
                && a.GetDirection(m) == m.GetDirection(x)
            );
        var aPredicate = new Func<CharPoint, CharPoint, CharPoint, bool>((x, m, a) =>
            a.Character == 'A'
                && Distance(m, a) == 1
                && Distance(x, a) == 2
                && characterGrid.Any((s) => sPredicate(x, m, a, s))
            );
        var mPredicate = new Func<CharPoint, CharPoint, bool>((x, m) =>
            m.Character == 'M' && Distance(x, m) == 1
                && characterGrid.Any((a) => aPredicate(x, m, a))
            );
        var xPredicate = new Func<CharPoint, bool>((x) =>
            x.Character == 'X'
                && characterGrid.Any((m) => mPredicate(x, m))
            );

        // Get all X chars that participate and count the ones that start multiple matches after.
        var validXChars = characterGrid.Where(xPredicate).ToArray();

        var result = 0;
        foreach (var x in validXChars)
        {
            // Count all Ms that lead to As and finally to Ss. Counting Ms from
            // the already overlapping Xs is enough as overlap cannot occur on both X and M simultaneously.
            result += characterGrid.Count((m) => mPredicate(x, m));
        }

        return result;
    }

    private int Distance(CharPoint point1, CharPoint point2) => Distance(point1.Position.X, point1.Position.Y, point2.Position.X, point2.Position.Y);

    private int Distance(int x1, int y1, int x2, int y2) => Math.Max(Math.Abs(x2 - x1), Math.Abs(y2 - y1));

    public record CharPoint(char Character, Vector Position)
    {
        public Vector GetDirection(CharPoint other)
        {
            return new Vector(other.Position.X - Position.X, other.Position.Y - Position.Y);
        }
    }

    public record Vector(int X, int Y)
    {
        public static Vector operator +(Vector left, Vector right) => new Vector(left.X + right.X, left.Y + right.Y);
        public static Vector operator -(Vector left, Vector right) => new Vector(left.X - right.X, left.Y - right.Y);
    }
}
