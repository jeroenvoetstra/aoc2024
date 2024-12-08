using AoC24.Solutions.Day06.Models;

namespace AoC24.Solutions.Day06;

public partial class Part2(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0601.txt";
    private readonly string _filePath = filePath;

    public Part2()
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
            throw new InvalidOperationException();

        var (originPosition, originOrientation) = (guard.Position, guard.Orientation);
        var originalRoute = new HashSet<Point>(guard!.GetRouteUntilOut(lab).ToArray().Skip(1).Select((item) => item.Tile.Position));
        var results = new HashSet<Point>();
        foreach (var position in originalRoute)
        {
            if (!lab.IsInBounds(position) || lab.Grid[position.Y][position.X].TileType == TileType.Obstacle)
                continue;

            var backupTileType = lab.Grid[position.Y][position.X].TileType;
            lab.Grid[position.Y][position.X].ChangeType(TileType.Obstacle);
            guard = new Guard(originPosition, originOrientation);

            if (guard.WillGetStuckInLoop(lab))
            {
                results.Add(position);
            }
            lab.Grid[position.Y][position.X].ChangeType(backupTileType);
        }

        return results.Count;
    }
}
