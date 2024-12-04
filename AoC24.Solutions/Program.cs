using AoC24.Solutions;
using CurrentDay = AoC24.Solutions.Day04;

Environment.SetEnvironmentVariable("AOC_HOME", @"C:\dev\personal\aoc2024", EnvironmentVariableTarget.Process);

#if DEBUG


Console.WriteLine(AoCSolutionFactory.GetResult<CurrentDay.Part1>());
Console.WriteLine(AoCSolutionFactory.GetResult<CurrentDay.Part2>());

#else

BenchmarkDotNet.Running.BenchmarkRunner.Run<CurrentDay.Part1>();

#endif

