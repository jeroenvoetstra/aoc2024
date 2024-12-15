namespace AoC24.Solutions.Day15;

public partial class Part2(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\1501.txt";
    private readonly string _filePath = filePath;

    public Part2()
        : this(FilePath)
    { }

    public long GetResult() => 0L;
}
