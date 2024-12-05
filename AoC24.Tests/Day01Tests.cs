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

        var result = subject.GetResult();

        Assert.Equal(11, result);
    }

    [Fact]
    public void Part1_AdditionalMethods_Method1_WithExampleData()
    {
        var subject = new Part1(ExampleDataPath);

        var result = subject.Method1();

        Assert.Equal(11, result);
    }

    [Fact]
    public void Part1_AdditionalMethods_Method2_WithExampleData()
    {
        var subject = new Part1(ExampleDataPath);

        var result = subject.Method2();

        Assert.Equal(11, result);
    }

    [Fact]
    public void Part1_AdditionalMethods_Method3_WithExampleData()
    {
        var subject = new Part1(ExampleDataPath);

        var result = subject.Method3();

        Assert.Equal(11, result);
    }

    [Fact]
    public void Part1_WithRealData()
    {
        var subject = new Part1();

        var result = subject.GetResult();

        Assert.Equal(1830467, result);
    }

    [Fact]
    public void Part1_AdditionalMethods_Method1_WithRealData()
    {
        var subject = new Part1();

        var result = subject.Method1();

        Assert.Equal(1830467, result);
    }

    [Fact]
    public void Part1_AdditionalMethods_Method2_WithRealData()
    {
        var subject = new Part1();

        var result = subject.Method2();

        Assert.Equal(1830467, result);
    }

    [Fact]
    public void Part1_AdditionalMethods_Method3_WithRealData()
    {
        var subject = new Part1();

        var result = subject.Method3();

        Assert.Equal(1830467, result);
    }

    [Fact]
    public void Part2_WithExampleData()
    {
        var subject = new Part2(ExampleDataPath);

        var result = subject.GetResult();

        Assert.Equal(31, result);
    }

    [Fact]
    public void Part2_AdditionalMethods_Method1_WithExampleData()
    {
        var subject = new Part2(ExampleDataPath);

        var result = subject.Method1();

        Assert.Equal(31, result);
    }

    [Fact]
    public void Part2_WithRealData()
    {
        var subject = new Part2();

        var result = subject.GetResult();

        Assert.Equal(26674158, result);
    }

    [Fact]
    public void Part2_AdditionalMethods_Method1_WithRealData()
    {
        var subject = new Part2();

        var result = subject.Method1();

        Assert.Equal(26674158, result);
    }
}
