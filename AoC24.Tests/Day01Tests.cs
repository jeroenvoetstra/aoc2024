using AoC24.Solutions.Day01;
using Xunit;

namespace AoC24.Tests;

public class Day01Tests : UnitTestBase
{
    private static readonly string ExampleDataPath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0101.test.txt";

    [Fact]
    public void Part1_WithExampleData()
    {
        var subject = new Part1(ExampleDataPath);

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(11, operation.Result);
    }

    [Fact]
    public void Part1_WithRealData()
    {
        var subject = new Part1();

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(1830467, operation.Result);
    }

    [Fact]
    public void Part2_WithExampleData()
    {
        var subject = new Part2(ExampleDataPath);

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(31, operation.Result);
    }

    [Fact]
    public void Part2_WithRealData()
    {
        var subject = new Part2();

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(26674158, operation.Result);
    }
}
