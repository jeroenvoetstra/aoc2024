using AoC24.Solutions.Day10;
using Xunit;

namespace AoC24.Tests;

public class Day10Tests : UnitTestBase
{
    private static readonly string ExampleDataPath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\1001.test.txt";

    [Fact]
    public void Part1_WithExampleData()
    {
        var subject = new Part1(ExampleDataPath);

        var result = subject.GetResult();

        Assert.Equal(36, result);
    }

    [Fact]
    public void Part1_WithRealData()
    {
        var subject = new Part1();

        var result = subject.GetResult();

        Assert.Equal(789, result);
    }

    [Fact]
    public void Part2_WithExampleData()
    {
        var subject = new Part2(ExampleDataPath);

        var result = subject.GetResult();

        Assert.Equal(81, result);
    }

    [Fact]
    public void Part2_WithRealData()
    {
        var subject = new Part2();

        var result = subject.GetResult();

        Assert.Equal(1735, result);
    }
}
