using AoC24.Solutions.Day12;
using Xunit;

namespace AoC24.Tests;

public class Day12Tests : UnitTestBase
{
    private const string Example1DataPath = @"Input\test.1201.txt";
    private const string Example2DataPath = @"Input\test.1202.txt";
    private const string Example3DataPath = @"Input\test.1203.txt";
    private const string Example4DataPath = @"Input\test.1204.txt";
    private const string Example5DataPath = @"Input\test.1205.txt";

    [Theory]
    [InlineData(Example1DataPath, 140)]
    [InlineData(Example2DataPath, 772)]
    [InlineData(Example3DataPath, 1930)]
    public void Part1_WithExampleData(string dataPath, long expected)
    {
        var path = Path.Combine(Environment.GetEnvironmentVariable("AOC_HOME", EnvironmentVariableTarget.Process)!, dataPath);
        var subject = new Part1(path);

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(expected, operation.Result);
        AssertWithinTimeLimit(operation);
    }

    [Fact]
    public void Part1_WithRealData()
    {
        var subject = new Part1();

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(1471452, operation.Result);
        AssertWithinTimeLimit(operation);
    }

    [Theory]
    [InlineData(Example1DataPath, 80)]
    [InlineData(Example2DataPath, 436)]
    [InlineData(Example3DataPath, 1206)]
    [InlineData(Example4DataPath, 236)]
    [InlineData(Example5DataPath, 368)]
    public void Part2_WithExampleData(string dataPath, long expected)
    {
        var path = Path.Combine(Environment.GetEnvironmentVariable("AOC_HOME", EnvironmentVariableTarget.Process)!, dataPath);
        var subject = new Part2(path);

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(expected, operation.Result);
        AssertWithinTimeLimit(operation);
    }

    [Fact]
    public void Part2_WithRealData()
    {
        var subject = new Part2();

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(863366, operation.Result);
        AssertWithinTimeLimit(operation);
    }
}
