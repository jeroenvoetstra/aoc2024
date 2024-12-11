using AoC24.Solutions.Day03;
using Xunit;

namespace AoC24.Tests;

public class Day03Tests : UnitTestBase
{
    private static readonly string ExampleData1Path = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0301.test.txt";
    private static readonly string ExampleData2Path = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0302.test.txt";

    [Fact]
    public void Part1_WithExampleData()
    {
        var subject = new Part1(ExampleData1Path);

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(161, operation.Result);
        AssertWithinTimeLimit(operation);
    }

    [Fact]
    public void Part1_WithRealData()
    {
        var subject = new Part1();

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(163931492, operation.Result);
        AssertWithinTimeLimit(operation);
    }

    [Fact]
    public void Part2_WithExampleData()
    {
        var subject = new Part2(ExampleData2Path);

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(48, operation.Result);
        AssertWithinTimeLimit(operation);
    }

    [Fact]
    public void Part2_WithRealData()
    {
        var subject = new Part2();

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(76911921, operation.Result);
        AssertWithinTimeLimit(operation);
    }
}
