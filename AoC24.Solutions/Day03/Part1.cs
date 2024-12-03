using System.Text.RegularExpressions;

namespace AoC24.Solutions.Day03;

public class Part1 : IAoCSolution
{
    static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0301.txt";

    private static readonly Regex MultiplyPattern = new Regex(@"((mul\((?<num1>\d+)\,(?<num2>\d+)\)))");

    public long GetResult()
    {
        var content = File.ReadAllText(FilePath);
        var matches = MultiplyPattern.Matches(content);

        var result = 0L;
        foreach (Match match in matches)
        {
            var num1 = Convert.ToInt32(match.Groups["num1"].Value);
            var num2 = Convert.ToInt32(match.Groups["num2"].Value);
            result += num1 * num2;

        }

        return result;
    }
}
