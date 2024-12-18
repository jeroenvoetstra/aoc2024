using AoC24.Solutions.Day17.Models;

namespace AoC24.Solutions.Day17;

public class Part2(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\1701.txt";
    private readonly string _filePath = filePath;

    public Part2()
        : this(FilePath)
    { }

    public long GetResult()
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

        // TODO: work backwards towards register A

        return 0L;
    }
}
