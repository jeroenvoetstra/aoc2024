using AoC24.Solutions.Day17.Models;

namespace AoC24.Solutions.Day17;

public class Part1(string filePath) : IAoCSolution<string>
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\1701.txt";
    private readonly string _filePath = filePath;

    public Part1()
        : this(FilePath)
    { }

    public string GetResult()
    {
        var input = File.ReadAllText(_filePath);
        var match = Patterns.InitializationInfo().Match(input);

        if (!match.Success)
            throw new InvalidOperationException();

        var computer = new ChronospatialComputer(
            Convert.ToInt32(match.Groups["a"].Value),
            Convert.ToInt32(match.Groups["b"].Value),
            Convert.ToInt32(match.Groups["c"].Value)
            );

        computer.LoadProgram(match.Groups["program"].Value.Split(',').Select((d) => Convert.ToByte(d)).ToArray());

        return string.Join(",", computer.GetOutput());
    }
}
