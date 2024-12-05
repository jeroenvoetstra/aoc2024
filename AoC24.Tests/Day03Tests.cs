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

        var result = subject.GetResult();

        Assert.Equal(161, result);
    }

    [Fact]
    public void Part1_AdditionalMethods_GetMuls_WithExampleData()
    {
        var subject = new Part1(ExampleData1Path);

        var result = subject.GetMuls();

        Assert.Equal(161, result);
    }

    [Fact]
    public void Part1_AdditionalMethods_AllOperationsWithGeneratedRegex_WithExampleData()
    {
        var subject = new Part1(ExampleData1Path);

        var result = subject.AllOperationsWithGeneratedRegex();

        Assert.Equal(161, result);
    }

    [Fact]
    public void Part1_AdditionalMethods_AllOperationsWithRegularRegex_WithExampleData()
    {
        var subject = new Part1(ExampleData1Path);

        var result = subject.AllOperationsWithRegularRegex();

        Assert.Equal(161, result);
    }

    [Fact]
    public void Part1_WithRealData()
    {
        var subject = new Part1();

        var result = subject.GetResult();

        Assert.Equal(163931492, result);
    }

    [Fact]
    public void Part1_AdditionalMethods_GetMuls_WithRealData()
    {
        var subject = new Part1();

        var result = subject.GetMuls();

        Assert.Equal(163931492, result);
    }

    [Fact]
    public void Part1_AdditionalMethods_AllOperationsWithGeneratedRegex_WithRealData()
    {
        var subject = new Part1();

        var result = subject.AllOperationsWithGeneratedRegex();

        Assert.Equal(163931492, result);
    }

    [Fact]
    public void Part1_AdditionalMethods_AllOperationsWithRegularRegex_WithRealData()
    {
        var subject = new Part1();

        var result = subject.AllOperationsWithRegularRegex();

        Assert.Equal(163931492, result);
    }

    [Fact]
    public void Part2_WithExampleData()
    {
        var subject = new Part2(ExampleData2Path);

        var result = subject.GetResult();

        Assert.Equal(48, result);
    }

    [Fact]
    public void Part2_WithRealData()
    {
        var subject = new Part2();

        var result = subject.GetResult();

        Assert.Equal(76911921, result);
    }
}
