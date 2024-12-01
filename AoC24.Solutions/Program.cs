using AoC = AoC24.Solutions;

Environment.SetEnvironmentVariable("AOC_HOME", @"C:\dev\personal\aoc2024", EnvironmentVariableTarget.Process);

#if DEBUG


Console.WriteLine(AoC.AoCSolutionFactory.GetResult<AoC.Day01.Part1>());
Console.WriteLine(AoC.AoCSolutionFactory.GetResult<AoC.Day01.Part2>());

#else

BenchmarkDotNet.Running.BenchmarkRunner.Run<AoC.Day01.Part1>();

#endif

