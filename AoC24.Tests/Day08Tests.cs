using AoC24.Solutions.Day08;
using Xunit;

namespace AoC24.Tests;

public class Day08Tests : UnitTestBase
{
    private static readonly string ExampleDataPath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0801.test.txt";

    [Fact]
    public void Part1_WithExampleData()
    {
        var subject = new Part1(ExampleDataPath);

        var result = subject.GetResult();

        Assert.Equal(14, result);
    }

    [Fact]
    public void Part1_WithRealData()
    {
        var subject = new Part1();

        var result = subject.GetResult();

        Assert.Equal(359, result);
    }

    [Fact]
    public void Part2_WithExampleData()
    {
        var subject = new Part2(ExampleDataPath);

        var result = subject.GetResult();

        Assert.Equal(34, result);
    }

    [Fact]
    public void Part2_WithRealData()
    {
        var subject = new Part2();

        var result = subject.GetResult();

        Assert.Equal(1293, result);
    }
}
