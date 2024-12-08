﻿using AoC24.Solutions.Day07;
using Xunit;

namespace AoC24.Tests;

public class Day07Tests : UnitTestBase
{
    private static readonly string ExampleDataPath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0701.test.txt";

    [Fact]
    public void Part1_WithExampleData()
    {
        var subject = new Part1(ExampleDataPath);

        var result = subject.GetResult();

        Assert.Equal(3749, result);
    }

    [Fact]
    public void Part1_WithRealData()
    {
        var subject = new Part1();

        var result = subject.GetResult();

        Assert.Equal(2664460013123, result);
    }

    [Fact]
    public void Part2_WithExampleData()
    {
        var subject = new Part2(ExampleDataPath);

        var result = subject.GetResult();

        Assert.Equal(11387, result);
    }

    [Fact]
    public void Part2_WithRealData()
    {
        var subject = new Part2();

        var result = subject.GetResult();

        Assert.Equal(426214131924213, result);
    }
}