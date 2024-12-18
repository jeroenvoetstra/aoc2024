using AoC24.Solutions.Day16;
using Xunit;

namespace AoC24.Tests;

public class Day16Tests : UnitTestBase
{
    private const int Unknown = -1;

    private const string Example1DataPath = @"Input\test.1601.txt";
    private const string Example2DataPath = @"Input\test.1602.txt";
    private const string Example3DataPath = @"Input\test.1603.txt";
    private const string Example4DataPath = @"Input\test.1604.txt";
    private const string Example5DataPath = @"Input\test.1605.txt";
    private const string Example6DataPath = @"Input\test.1606.txt";
    private const string Example7DataPath = @"Input\test.1607.txt";
    private const string Example8DataPath = @"Input\test.1608.txt";

    [Theory]
    [InlineData(Example1DataPath, 7036)]
    [InlineData(Example2DataPath, 11048)]
    [InlineData(Example3DataPath, 3010)]
    [InlineData(Example4DataPath, 21148)]
    [InlineData(Example5DataPath, 5078)] // test case with large open space, should be done with A* or Dijkstra
    [InlineData(Example6DataPath, 21110)] // test case with large open space, should be done with A* or Dijkstra
    [InlineData(Example7DataPath, 41210)] // test case with large open space, should be done with A* or Dijkstra
    [InlineData(Example8DataPath, 4013)]
    public void Part1_WithExampleData(string dataPath, long expected)
    {
        var path = Path.Combine(Environment.GetEnvironmentVariable("AOC_HOME", EnvironmentVariableTarget.Process)!, dataPath);
        var subject = new Part1(path);

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(expected, operation.Result);
    }

    [Fact]
    public void Part1_WithRealData()
    {
        var subject = new Part1();

        var operation = TimeOperation(subject.GetResult);

        // 127452 too high
        // 105512 too high
        Assert.Equal(-1, operation.Result);
    }

    [Theory]
    [InlineData(Example1DataPath, Unknown)]
    [InlineData(Example2DataPath, Unknown)]
    [InlineData(Example3DataPath, Unknown)]
    [InlineData(Example4DataPath, 149)]
    [InlineData(Example5DataPath, 413)]
    [InlineData(Example6DataPath, 264)]
    [InlineData(Example7DataPath, 514)]
    public void Part2_WithExampleData(string dataPath, long expected)
    {
        var path = Path.Combine(Environment.GetEnvironmentVariable("AOC_HOME", EnvironmentVariableTarget.Process)!, dataPath);
        var subject = new Part2(path);

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(expected, operation.Result);
    }

    [Fact]
    public void Part2_WithRealData()
    {
        var subject = new Part2();

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(Unknown, operation.Result);
    }
}
