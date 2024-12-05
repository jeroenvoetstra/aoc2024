namespace AoC24.Solutions.Day06;

public partial class Part2(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0501.txt";
    private readonly string _filePath = filePath;

    public Part2()
        : this(FilePath)
    { }

    public long GetResult()
    {
        Console.WriteLine(File.Exists(_filePath));
        return 0L;
    }
}
