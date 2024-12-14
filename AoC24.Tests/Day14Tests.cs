using AoC24.Solutions.Day14;
using Xunit;

namespace AoC24.Tests;

public class Day14Tests : UnitTestBase
{
    private static readonly string ExampleDataPath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\test.1401.txt";

    [Fact]
    public void Part1_WithExampleData()
    {
        var subject = new Part1(ExampleDataPath, 100, 11, 7);

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(12, operation.Result);
        AssertWithinTimeLimit(operation);
    }

    [Fact]
    public void Part1_WithRealData()
    {
        var subject = new Part1();

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(223020000, operation.Result);
        AssertWithinTimeLimit(operation);
    }

    [Fact]
    public void Part2_WithRealData()
    {
        var subject = new Part2();

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(7338, operation.Result);
        AssertWithinTimeLimit(operation, 2000);
    }
}
