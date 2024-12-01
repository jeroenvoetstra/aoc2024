using AoC24.Solutions;
using AoC = AoC24.Solutions;


#if DEBUG


Console.WriteLine(AoCSolutionFactory.GetResult<AoC.Day01.Part1>());
Console.WriteLine(AoCSolutionFactory.GetResult<AoC.Day01.Part2>());

#else

BenchmarkDotNet.Running.BenchmarkRunner.Run<AoC.Day01.Part1>();

#endif

