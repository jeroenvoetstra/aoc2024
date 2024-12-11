using BenchmarkDotNet.Attributes;
using System.Collections.Concurrent;

namespace AoC24.Solutions.Day11;

/* BENCHMARK RESULTS
// * Summary *

BenchmarkDotNet v0.14.0, Windows 10 (10.0.19045.5131/22H2/2022Update)
11th Gen Intel Core i7-1185G7 3.00GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK 9.0.101
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


| Method                    | Mean      | Error     | StdDev    | Gen0       | Completed Work Items | Lock Contentions | Gen1     | Gen2    | Allocated |
|-------------------------- |----------:|----------:|----------:|-----------:|---------------------:|-----------------:|---------:|--------:|----------:|
| Optimized                 |  1.854 ms | 0.0555 ms | 0.1592 ms |   269.5313 |                    - |                - | 113.2813 | 89.8438 |   1.54 MB |
| Unoptimized               | 58.473 ms | 1.0587 ms | 0.8841 ms | 10444.4444 |                    - |                - | 111.1111 |       - |     63 MB |
| MultithreadingOptimized   |  2.378 ms | 0.0468 ms | 0.0945 ms |   296.8750 |               8.0000 |          14.2656 | 171.8750 | 85.9375 |   1.91 MB |
| MultithreadingUnoptimized | 29.374 ms | 0.5778 ms | 0.9331 ms | 10562.5000 |               8.0000 |                - | 718.7500 |       - |     63 MB |
 */

[MemoryDiagnoser]
[ThreadingDiagnoser]
public partial class Part1(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\1101.txt";
    private readonly string _filePath = filePath;

    public Part1()
        : this(FilePath)
    { }

    public long GetResult() => Optimized();

    [Benchmark]
    public long Optimized()
    {
        var items = File.ReadAllText(_filePath).Split(' ');

        var result = 0L;
        var cache = new Dictionary<string, long>();
        foreach (var item in items)
        {
            result += GetItems([item], 0, 25, cache).Sum();
        }

        return result;
    }

    [Benchmark]
    public long Unoptimized()
    {
        var items = File.ReadAllText(_filePath).Split(' ');

        var result = 0L;
        foreach (var item in items)
        {
            result += GetItemsUnOptimized([item], 0, 25).Count();
        }

        return result;
    }

    [Benchmark]
    public long MultithreadingOptimized()
    {
        var sync = new object();
        var items = File.ReadAllText(_filePath).Split(' ');

        var result = 0L;
        var cache = new ConcurrentDictionary<string, long>();
        var waitHandles = new List<AutoResetEvent>();
        foreach (var item in items)
        {
            var wait = new AutoResetEvent(false);
            waitHandles.Add(wait);
            ThreadPool.QueueUserWorkItem((state) =>
            {
                var r = GetItems([item], 0, 25, cache).Sum();
                lock (sync)
                {
                    result += r;
                }
                wait.Set();
            });
        }
        waitHandles.ForEach((handle) => handle.WaitOne());

        return result;
    }

    [Benchmark]
    public long MultithreadingUnoptimized()
    {
        var sync = new object();
        var items = File.ReadAllText(_filePath).Split(' ');

        var result = 0L;
        var waitHandles = new List<AutoResetEvent>();
        foreach (var item in items)
        {
            var wait = new AutoResetEvent(false);
            waitHandles.Add(wait);
            ThreadPool.QueueUserWorkItem((state) =>
            {
                var r = GetItemsUnOptimized([item], 0, 25).Count();
                lock (sync)
                {
                    result += r;
                }
                wait.Set();
            });
        }
        waitHandles.ForEach((handle) => handle.WaitOne());

        return result;
    }

    internal static IEnumerable<long> GetItems(IEnumerable<string> items, int iteration, int maxIterations, IDictionary<string, long> cache)
    {
        if (iteration >= maxIterations)
        {
            // We actually don't need the array at all, the description is misleading.
            yield return items.Count();
        }
        else
        {
            foreach (var item in items)
            {
                // We have to include the iteration in the key in order to properly store a count
                var cacheKey = $"{item}|{iteration}";
                if (cache.ContainsKey(cacheKey))
                {
                    yield return cache[cacheKey];
                }
                else
                {
                    string[] nextIteration;
                    if (item == "0")
                    {
                        nextIteration = ["1"];
                    }
                    else if ((item.Length % 2) == 0)
                    {
                        nextIteration = [
                            Convert.ToInt64(item.Substring(0, item.Length / 2)).ToString(),
                            Convert.ToInt64(item.Substring(item.Length / 2)).ToString(),
                        ];
                    }
                    else
                    {
                        nextIteration = [(Convert.ToInt64(item) * 2024).ToString()];
                    }

                    var result = GetItems(nextIteration, iteration + 1, maxIterations, cache).Sum();
                    // Cache so if we run into this instance again we can swiftly return it.
                    cache[cacheKey] = result;
                    yield return result;
                }
            }
        }
    }

    internal static IEnumerable<string> GetItemsUnOptimized(IEnumerable<string> items, int iteration, int maxIterations)
    {
        if (iteration >= maxIterations)
        {
            foreach (var item in items)
            {
                yield return item;
            }
        }
        else
        {
            foreach (var item in items)
            {
                string[] nextIteration;
                if (item == "0")
                {
                    nextIteration = ["1"];
                }
                else if ((item.Length % 2) == 0)
                {
                    nextIteration = [
                        Convert.ToInt64(item.Substring(0, item.Length / 2)).ToString(),
                        Convert.ToInt64(item.Substring(item.Length / 2)).ToString(),
                        ];
                }
                else
                {
                    nextIteration = [(Convert.ToInt64(item) * 2024).ToString()];
                }

                foreach (var next in GetItemsUnOptimized(nextIteration, iteration + 1, maxIterations))
                {
                    yield return next;
                }
            }
        }
    }
}
