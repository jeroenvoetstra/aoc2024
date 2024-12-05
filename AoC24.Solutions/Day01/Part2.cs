using BenchmarkDotNet.Attributes;
using System.Collections.Immutable;

namespace AoC24.Solutions.Day01;

public class Part2(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0101.txt";
    private readonly string _filePath = filePath;

    public Part2()
        : this(FilePath)
    { }

    public long GetResult() => Method1();

    [Benchmark]
    public long Method1()
    {
        var pairs = File.ReadAllLines(_filePath)
            .Where((line) => !string.IsNullOrWhiteSpace(line))
            .Select((line) => line.Split(' ').Where((item) => !string.IsNullOrEmpty(item)).Select((item) => Convert.ToInt32(item.Trim())).ToArray())
            .ToArray();
        var left = Enumerable.Range(0, pairs.Length).Select((index) => pairs[index][0]).ToImmutableArray();
        var right = Enumerable.Range(0, pairs.Length).Select((index) => pairs[index][1]).ToImmutableArray();

        var solution = 0;
        foreach (var leftItem in left)
        {
            solution += leftItem * right.Count((item) => item == leftItem);
        }

        return solution;
    }
}
