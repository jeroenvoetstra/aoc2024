using AoC24.Solutions.Day17;
using Xunit;

namespace AoC24.Tests;

public class Day17Tests : UnitTestBase
{
    private const string Example1DataPath = @"Input\test.1701.txt";
    private const string Example2DataPath = @"Input\test.1702.txt";

    [Theory]
    [InlineData(Example1DataPath, "4,6,3,5,6,3,5,2,1,0")]
    [InlineData(Example2DataPath, "4,2,5,6,7,7,7,7,3,1,0")]
    public void Part1_WithExampleData(string dataPath, string expected)
    {
        var path = Path.Combine(Environment.GetEnvironmentVariable("AOC_HOME")!, dataPath);
        var subject = new Part1(path);

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(expected, operation.Result);
    }

    [Fact]
    public void Part1_WithRealData()
    {
        var subject = new Part1();

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal("2,1,4,0,7,4,0,2,3", operation.Result);
    }

    [Fact]
    public void Part2_WithExampleData()
    {
        var subject = new Part2(Example2DataPath);

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(117440, operation.Result);
    }

    [Fact]
    public void Part2_WithRealData()
    {
        var subject = new Part2();

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(-1, operation.Result);
    }
}
