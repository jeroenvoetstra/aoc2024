namespace AoC24.Solutions.Day10;

public partial class Part2(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\1001.txt";
    private readonly string _filePath = filePath;

    public Part2()
        : this(FilePath)
    { }

    public long GetResult() => Method2();

    // Depth-first search
    public long Method1() => new Part1(_filePath).Dfs().Count();
    // Breadth-first search
    public long Method2() => new Part1(_filePath).Bfs().Count();
}
