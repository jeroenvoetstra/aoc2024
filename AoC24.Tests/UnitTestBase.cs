namespace AoC24.Tests;

public class UnitTestBase
{
    public UnitTestBase()
    {
        Environment.SetEnvironmentVariable("AOC_HOME", @"C:\dev\personal\aoc2024", EnvironmentVariableTarget.Process);
    }
}
