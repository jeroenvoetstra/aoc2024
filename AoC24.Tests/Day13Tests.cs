using AoC24.Solutions.Day13;
using Xunit;

namespace AoC24.Tests;

public class Day13Tests : UnitTestBase
{
    private static readonly string ExampleDataPath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\test.1301.txt";

    [Fact]
    public void Part1_WithExampleData()
    {
        var subject = new Part1(ExampleDataPath);

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(480, operation.Result);
        AssertWithinTimeLimit(operation);
    }

    [Fact]
    public void Part1_WithRealData()
    {
        var subject = new Part1();

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(29517, operation.Result);
        AssertWithinTimeLimit(operation);
    }

    [Fact]
    public void Part2_WithExampleData()
    {
        var subject = new Part2(ExampleDataPath);

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(875318608908, operation.Result);
        AssertWithinTimeLimit(operation);
    }

    [Fact]
    public void Part2_WithRealData()
    {
        var subject = new Part2();

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(103570327981381, operation.Result);
        AssertWithinTimeLimit(operation);
    }
}
