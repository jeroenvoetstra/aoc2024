using System.Text.RegularExpressions;

namespace AoC24.Solutions.Day03;

public class Part2(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0301.txt";
    private readonly string _filePath = filePath;

    private static readonly Regex MultiplyPattern = new Regex(@"((mul\((?<num1>\d+)\,(?<num2>\d+)\))|((?<on>do\(\))|(?<off>don\'t\(\))))");

    public Part2()
        : this(FilePath)
    { }

    public long GetResult()
    {
        var content = File.ReadAllText(_filePath);
        var matches = MultiplyPattern.Matches(content);

        var result = 0L;
        var enabled = true;
        foreach (Match match in matches)
        {
            var on = match.Groups["on"].Value;
            var off = match.Groups["off"].Value;
            if (!string.IsNullOrEmpty(on))
            {
                enabled = true;
                continue;
            }
            if (!string.IsNullOrEmpty(off))
            {
                enabled = false;
                continue;
            }

            var num1 = Convert.ToInt32(match.Groups["num1"].Value);
            var num2 = Convert.ToInt32(match.Groups["num2"].Value);

            if (enabled)
                result += num1 * num2;

        }

        return result;
    }
}
