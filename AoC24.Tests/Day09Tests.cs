﻿using AoC24.Solutions.Day09;
using Xunit;

namespace AoC24.Tests;

public class Day09Tests : UnitTestBase
{
    private static readonly string ExampleDataPath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0901.test.txt";

    [Fact]
    public void Part1_WithExampleData()
    {
        var subject = new Part1(ExampleDataPath);

        var result = subject.GetResult();

        Assert.Equal(1928, result);
    }

    [Fact]
    public void Part1_WithRealData()
    {
        var subject = new Part1();

        var result = subject.GetResult();

        Assert.Equal(6242766523059, result);
    }

    [Fact]
    public void Part2_WithExampleData()
    {
        var subject = new Part2(ExampleDataPath);

        var result = subject.GetResult();

        Assert.Equal(0, result);
        //Assert.Equal(2858, result);
    }

    [Fact]
    public void Part2_WithRealData()
    {
        var subject = new Part2();

        var result = subject.GetResult();

        Assert.Equal(0, result);
    }
}