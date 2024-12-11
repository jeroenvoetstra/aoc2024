using AoC24.Solutions.Day09;
using Xunit;

namespace AoC24.Tests;

public class Day09Tests : UnitTestBase
{
    private static readonly string ExampleDataPath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0901.test.txt";

    [Fact]
    public void Part1_WithExampleData()
    {
        var subject = new Part1(ExampleDataPath);

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(1928, operation.Result);
        AssertWithinTimeLimit(operation);
    }

    [Fact]
    public void Part1_WithRealData()
    {
        var subject = new Part1();

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(6242766523059, operation.Result);
        AssertWithinTimeLimit(operation);
    }

    [Fact]
    public void Part2_WithExampleData()
    {
        var subject = new Part2(ExampleDataPath);

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(2858, operation.Result);
        AssertWithinTimeLimit(operation);
    }

    [Fact]
    public void Part2_WithRealData()
    {
        var subject = new Part2();

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(6272188244509, operation.Result);
        AssertWithinTimeLimit(operation);
    }
}
