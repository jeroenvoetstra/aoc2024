using AoC24.Solutions;
using System.Reflection;
using CurrentDay = AoC24.Solutions.Day06;

Environment.SetEnvironmentVariable("AOC_HOME", @"C:\dev\personal\aoc2024", EnvironmentVariableTarget.Process);

#if DEBUG

Console.WriteLine(AoCSolutionFactory.GetResult<CurrentDay.Part1>());
Console.WriteLine(AoCSolutionFactory.GetResult<CurrentDay.Part2>());

#elif BENCHMARK

BenchmarkDotNet.Running.BenchmarkRunner.Run<CurrentDay.Part1>();

#else

foreach (var type in Assembly.GetExecutingAssembly().GetTypes().OrderBy((type) => type.FullName))
{
    if (type.GetInterfaces().Contains(typeof(IAoCSolution)))
        Console.WriteLine("{0}: {1}", type.FullName, (Activator.CreateInstance(type) as IAoCSolution)!.GetResult());
}

#endif

