using System.Diagnostics;

namespace AoC24.Tests;

public class UnitTestBase
{
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

    public record TimedOperation<TResult>(TResult Result, long MillisecondsTaken);
}
