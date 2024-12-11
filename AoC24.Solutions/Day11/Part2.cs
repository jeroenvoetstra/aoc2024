using System.Collections.Concurrent;

namespace AoC24.Solutions.Day11;

public partial class Part2(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\1101.txt";
    private readonly string _filePath = filePath;

    public Part2()
        : this(FilePath)
    { }

    public long GetResult() => Optimized();

    public long Optimized()
    {
        var items = File.ReadAllText(_filePath).Split(' ');

        var result = 0L;
        var cache = new Dictionary<string, long>();
        foreach (var item in items)
        {
            result += Part1.GetItemsOptimized([item], 0, 75, cache).Sum();
        }

        return result;
    }

    public long MultithreadingOptimized()
    {
        var sync = new object();
        var items = File.ReadAllText(_filePath).Split(' ');

        var result = 0L;
        var cache = new ConcurrentDictionary<string, long>();
        var threadPool = new List<Thread>();
        foreach (var item in items)
        {
            var thread = new Thread(() =>
            {
                var r = Part1.GetItemsOptimized([item], 0, 75, cache).Sum();
                lock (sync)
                {
                    result += r;
                }
            });
            threadPool.Add(thread);
            thread.Start();
        }
        threadPool.ForEach((thread) => thread.Join());

        return result;
    }
}
