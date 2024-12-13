using AoC24.Solutions.Day13.Models;
using System.Text.RegularExpressions;

namespace AoC24.Solutions.Day13;

public partial class Part2(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\1301.txt";
    private readonly string _filePath = filePath;

    public Part2()
        : this(FilePath)
    { }

    public long GetResult() => InverseMatrixMultiplicationMethod();

    public long InverseMatrixMultiplicationMethod() => new Part1().InverseMatrixMultiplicationMethod(GetMachinesFromContent(File.ReadAllText(_filePath)));

    public long EliminationMethod() => new Part1().EliminationMethod(GetMachinesFromContent(File.ReadAllText(_filePath)));

    // The brute force method is not an option

    private static IEnumerable<Machine> GetMachinesFromContent(string content)
    {
        var results = Part1.ItemPattern()
            .Matches(content)
            .OfType<Match>()
            .Select((match) => new Machine(
                (Convert.ToInt64(match.Groups["a_x"].Value), Convert.ToInt64(match.Groups["a_y"].Value)),
                (Convert.ToInt64(match.Groups["b_x"].Value), Convert.ToInt64(match.Groups["b_y"].Value)),
                Convert.ToInt64(match.Groups["x"].Value) + 10000000000000,
                Convert.ToInt64(match.Groups["y"].Value) + 10000000000000
                )
            );
        foreach (var result in results)
        {
            yield return result;
        }
    }
}
