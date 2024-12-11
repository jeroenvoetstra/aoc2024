﻿using AoC24.Solutions.Day11;
using Xunit;

namespace AoC24.Tests;

public class Day11Tests : UnitTestBase
{
    private static readonly string ExampleDataPath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\1101.test.txt";
    private const int SelfImposedTimeLimit = 500;

    [Fact]
    public void Part1_WithExampleData()
    {
        var subject = new Part1(ExampleDataPath);

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(55312, operation.Result);
        Assert.True(operation.MillisecondsTaken < SelfImposedTimeLimit, "Solution took too long to process");
    }

    [Fact]
    public void Part1_WithRealData()
    {
        var subject = new Part1();

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(175006, operation.Result);
        Assert.True(operation.MillisecondsTaken < SelfImposedTimeLimit, "Solution took too long to process");
    }

    [Fact]
    public void Part2_WithExampleData()
    {
        var subject = new Part2(ExampleDataPath);

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(65601038650482, operation.Result);
        Assert.True(operation.MillisecondsTaken < SelfImposedTimeLimit, "Solution took too long to process");
    }

    [Fact]
    public void Part2_WithRealData()
    {
        var subject = new Part2();

        var operation = TimeOperation(subject.GetResult);

        Assert.Equal(207961583799296, operation.Result);
        Assert.True(operation.MillisecondsTaken < SelfImposedTimeLimit, "Solution took too long to process");
    }
}
