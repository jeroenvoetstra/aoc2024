using AoC24.Solutions.Day13.Models;
using System.Text.RegularExpressions;

namespace AoC24.Solutions.Day13;

public partial class Part1(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\1301.txt";
    private readonly string _filePath = filePath;

    private const int ButtonAValue = 3;
    private const int ButtonBValue = 1;
    private const int MaxPushes = 100;

    [GeneratedRegex(@"Button\sA\:\s(X\+(?<a_x>\d+))\,\s(Y\+(?<a_y>\d+))[\r\n\s]*Button\sB\:\s(X\+(?<b_x>\d+))\,\s(Y\+(?<b_y>\d+))[\r\n\s]*Prize\:\s(X\=(?<x>\d+))\,\s(Y\=(?<y>\d+))")]
    internal static partial Regex ItemPattern();

    public Part1()
        : this(FilePath)
    { }

    public long GetResult() => InverseMatrixMultiplicationMethod(GetMachinesFromContent(File.ReadAllText(_filePath)));

    public long InverseMatrixMultiplicationMethod(IEnumerable<Machine> machines)
    {
        /*
         * We'll use inverse matrix multiplication to solve the linear
         * equations and see if the results are integers. --> Correction: we will not check for integers
         *                                                     but check back to the result with the rounded
         *                                                     values. This way, any rounding error is mitigated.
         *                                                     E.g. 54.0000001 will not count if we do x % 1 == 0.
         * 
         * The linear equations we are trying to solve are:
         *  for X: R(x) = a⋅A(x) + b⋅B(x)
         *  for Y: R(y) = a⋅A(y) + b⋅B(y)
         * where:
         *  R(x), R(y) are the given target X and Y coordinates
         *  A(x), A(y) are the given X, Y movement that button A generate
         *  B(x), B(y) are the given X, Y movement that button B generate
         *  a, b are the times buttons A and B respectively are pressed
         * 
         * In matrix form
         * ┌            ┐ ┌   ┐   ┌      ┐
         * │ A(x)  B(x) │ │ a │   │ R(x) │
         * │ A(y)  B(y) │ │ b │ = │ R(y) │
         * └            ┘ └   ┘   └      ┘
         * We'll give them names:
         *        A          X        C
         * 
         * With matrix A:
         * ┌      ┐
         * │ a  b │
         * │ c  d │
         * └      ┘
         * The inverse of A is defined as:
         *          ┌       ┐
         *          │ d  -b │
         *  1 / |A| │ -c  a │ = A⁻¹
         *          └       ┘
         *          ┌             ┐
         *          │ B(y)  -B(x) │
         *  1 / |A| │ -A(y)  A(x) │
         *          └             ┘
         *     ┌                                ┐
         *     │ 1/|A| * B(y)     1/|A| * -B(x) │
         *  => │ 1/|A| * -A(y)    1/|A| *  A(x) │
         *     └                                ┘
         * 
         * A⋅X = C
         * When we multiply both sides with the inverse matrix of A:
         * A⁻¹⋅A⋅X = A⁻¹⋅C
         * It means that we can eliminate A from the left (a matrix multiplied by its inverse is the identity matrix):
         * X = A⁻¹⋅C
         * So now:
         * ┌   ┐   ┌                                ┐ ┌      ┐
         * │ a │   │ 1/|A| * B(y)     1/|A| * -B(x) │ │ R(x) │
         * │ b │ = │ 1/|A| * -A(y)    1/|A| *  A(x) │ │ R(y) │
         * └   ┘   └                                ┘ └      ┘
         * 
         * So a = (1/|A| * B(y) * R(x)) + (1/|A| * -B(x) * R(y))
         * and b = (1/|A| x -A(y) * R(x)) + (1/|A| *  A(x) * R(y))
         * where a = times button A is pressed, and b = times button B is pressed. 
         */

        var result = 0L;
        foreach (var machine in machines)
        {
            var matrix = new { A = machine.A.X, B = machine.B.X, C = machine.A.Y, D = machine.B.Y };

            var determinant = (matrix.A * matrix.D) - (matrix.C * matrix.B);

            var inverseMatrix = new double[2][];
            inverseMatrix[0] = [ (1d / determinant) * matrix.D, (1d / determinant) * (matrix.B * -1) ];
            inverseMatrix[1] = [ (1d / determinant) * (matrix.C * -1), (1d / determinant) * matrix.A ];

            var multipliedMatrix = new double[2];
            multipliedMatrix[0] = (inverseMatrix[0][0] * machine.X) + (inverseMatrix[0][1] * machine.Y);
            multipliedMatrix[1] = (inverseMatrix[1][0] * machine.X) + (inverseMatrix[1][1] * machine.Y);

            var answer = new { ButtonA = (long)Math.Round(multipliedMatrix[0], 0), ButtonB = (long)Math.Round(multipliedMatrix[1], 0) };

            var x = (answer.ButtonA * matrix.A) + (answer.ButtonB * matrix.B);
            var y = (answer.ButtonA * matrix.C) + (answer.ButtonB * matrix.D);
            if (x == machine.X && y == machine.Y)
            {
                result += (answer.ButtonA * ButtonAValue) + (answer.ButtonB * ButtonBValue);
            }
        }

        return result;
    }

    public long EliminationMethod(IEnumerable<Machine> machines)
    {
        var result = 0L;
        foreach (var machine in machines)
        {
            // Plain solving of 2-variable equation using elimination:
            /*
             * From the first example:
             * 94a + 22b = 8400
             * 34a + 67b = 5400
             * =>  67 * (94a + 22b) =  67 * 8400
             *    -22 * (34a + 67b) = -22 * 5400
             * =>  6298a + 1474b =  562800
             *     -748a - 1474b = -118800  +
             *     ==========================
             *     5550a +    0b =  444000
             * => b = 444000 / 5550 = 80
             * 
             * => (94 * 80) + 22b = 8400
             * => b = (8400 - 7520) / 22 = 40
             */
            var a = ((machine.B.Y * machine.X) + (-machine.B.X * machine.Y)) / ((machine.B.Y * machine.A.X) + (-machine.B.X * machine.A.Y));
            var b = (machine.X - (machine.A.X * a)) / machine.B.X;
            if ((machine.A.X * a) + (machine.B.X * b) == machine.X
                && (machine.A.Y * a) + (machine.B.Y * b) == machine.Y
                )
            {
                result += a * ButtonAValue + b * ButtonBValue;
            }
        }

        return result;
    }

    public long BruteForceMethod(IEnumerable<Machine> machines)
    {
        var result = 0L;
        foreach (var machine in machines)
        {
            result += BruteForce(machine);
        }

        return result;
    }

    private static IEnumerable<Machine> GetMachinesFromContent(string content)
    {
        var results = ItemPattern()
            .Matches(content)
            .OfType<Match>()
            .Select((match) => new Machine(
                (Convert.ToInt64(match.Groups["a_x"].Value), Convert.ToInt64(match.Groups["a_y"].Value)),
                (Convert.ToInt64(match.Groups["b_x"].Value), Convert.ToInt64(match.Groups["b_y"].Value)),
                Convert.ToInt64(match.Groups["x"].Value),
                Convert.ToInt64(match.Groups["y"].Value)
                )
            );
        foreach (var result in results)
        {
            yield return result;
        }
    }

    private static long BruteForce(Machine machine)
    {
        for (var a = 1; a <= MaxPushes; a++)
        {
            for (var b = 1; b <= MaxPushes; b++)
            {
                if ((a * machine.A.X) + (b * machine.B.X) == machine.X
                    && (a * machine.A.Y) + (b * machine.B.Y) == machine.Y
                    )
                {
                    return (a * ButtonAValue) + (b * ButtonBValue);
                }
            }
        }
        return 0L;
    }
}
