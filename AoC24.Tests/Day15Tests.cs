using AoC24.Solutions.Day15;
using Xunit;

namespace AoC24.Tests;

public class Day15Tests : UnitTestBase
{
    private static readonly string ExampleDataPath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\test.1501.txt";
    private static readonly string ExampleData2Path = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\test.1502.txt";

    [Fact]
    public void Part1_WithExampleData()
    {
        var subject = new Part1(ExampleDataPath);

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(10092, operation.Result);
    }

    [Fact]
    public void Part1_WithRealData()
    {
        var subject = new Part1();

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(1552463, operation.Result);
    }

    [Fact]
    public void Part2_WithExampleData()
    {
        var subject = new Part2(ExampleDataPath);

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(0, operation.Result);
    }

    [Fact]
    public void Part2_WithExampleData2()
    {
        var subject = new Part2(ExampleData2Path);

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(0, operation.Result);
    }

    [Fact]
    public void Part2_WithRealData()
    {
        var subject = new Part2();

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(0, operation.Result);
    }
}
