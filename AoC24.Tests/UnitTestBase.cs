using System.Diagnostics;
using Xunit;

namespace AoC24.Tests;

public class UnitTestBase
{
    protected const int SelfImposedTimeLimit = 1000;

    public UnitTestBase()
    {
        Environment.SetEnvironmentVariable("AOC_HOME", @"C:\dev\personal\aoc2024", EnvironmentVariableTarget.Process);
    }

    protected TimedOperation<TResult> TimeOperation<TResult>(Func<TResult> operation)
    {
        var stopwatch = Stopwatch.StartNew();
        var result = operation();
        stopwatch.Stop();

        return new TimedOperation<TResult>(result, stopwatch.ElapsedMilliseconds);
    }

    protected void AssertWithinTimeLimit<TResult>(TimedOperation<TResult> operation, int timeLimitMs = SelfImposedTimeLimit)
    {
        Assert.True(operation.MillisecondsTaken < timeLimitMs, $"Solution took too long to process ({operation.MillisecondsTaken:N0} > {timeLimitMs:N0} ms)");
    }

    public record TimedOperation<TResult>(TResult Result, long MillisecondsTaken);
}
