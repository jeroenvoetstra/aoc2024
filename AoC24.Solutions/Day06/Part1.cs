using AoC24.Solutions.Day06.Models;

namespace AoC24.Solutions.Day06;

public partial class Part1(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0601.txt";
    private readonly string _filePath = filePath;

    public Part1()
        : this(FilePath)
    { }

    public long GetResult() => Method1();

    public long Method1()
    {
        var input = File.ReadAllLines(_filePath);

        var guard = default(Guard);
        var lab = new Laboratory(
            input
                .Select((line, y) =>
                    line
                        .Select((c, x) =>
                        {
                            if (Guard.GuardOrientations.ContainsKey(c))
                                guard = new Guard(c, x, y);

                            return new Tile(c, x, y);
                        })
                        .ToArray()
                    )
                .ToArray()
            );

        if (guard == default)
            throw new InvalidOperationException("Where is the guard?");

        return guard.GetRouteUntilOut(lab)
            .Select((item) => (item.Tile.Position.X, item.Tile.Position.Y))
            .Distinct()
            .LongCount()
            ;
    }
}
