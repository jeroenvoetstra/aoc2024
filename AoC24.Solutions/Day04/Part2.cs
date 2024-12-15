using static AoC24.Solutions.Day04.Part1;

namespace AoC24.Solutions.Day04;

public partial class Part2(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0401.txt";
    private readonly string _filePath = filePath;

    public Part2()
        : this(FilePath)
    { }

    public long GetResult() => Method2();

    public long Method1()
    {
        var characterMatrix = File.ReadAllLines(_filePath);
        var characterGrid = characterMatrix.SelectMany((line, y) => line.Select((c, x) => new CharPoint(c, new Vector(x, y)))).ToArray();

        var results = characterGrid
            .Where((a) => a.Character == 'A')
            .Select((a) => new
            {
                Center = a,
                // Get the characters in each of the (diagonal) corners
                Satellites = characterGrid.Where((other) =>
                    other.Position == a.Position + new Vector(-1, -1)
                    || other.Position == a.Position + new Vector(1, -1)
                    || other.Position == a.Position + new Vector(-1, 1)
                    || other.Position == a.Position + new Vector(1, 1)
                    )
                .ToArray()
            })
            .Where((a) =>
                // Each successful X-MAS should have exactly 2 Ms and 2 Ss
                a.Satellites.Count((sat) => sat.Character == 'M') == 2
                    && a.Satellites.Count((sat) => sat.Character == 'S') == 2
                    // The above is not enough, make sure the Ms and/or the Ss are not on opposing corners (SAS/MAM combo). Took
                    // quite a while to realize this edge case.
                    // Solution: opposing corners cancel each other out when x and y are summed (e.g. (5, 3) and (3, 5) around (4, 4)),
                    // so we're looking for pairs that do not add to twice the sum of the center's position. Trust me, it checks out.
                    && a.Satellites.Where((sat) => sat.Character == 'M').Sum((sat) => sat.Position.X + sat.Position.Y) != (2 * (a.Center.Position.X + a.Center.Position.Y))
                // No need to check S as that is implied from the M check
                )
            .ToArray();

        return results.Length;
    }

    public long Method2()
    {
        var characterMatrix = File.ReadAllLines(_filePath)
            .Select((line) => line.Select((c) => c).ToArray()).ToArray();

        var characterPositionHashSet = new HashSet<(int x, int y, char c)>(characterMatrix.SelectMany((line, y) => line.Select((c, x) => (x, y, c))));

        var width = characterMatrix[0].Length;
        var height = characterMatrix.Length;

        var result = 0L;
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                if (characterMatrix[y][x] == 'A')
                {
                    if ((characterPositionHashSet.Contains((x - 1, y - 1, 'M')) && characterPositionHashSet.Contains((x + 1, y - 1, 'M'))
                            && characterPositionHashSet.Contains((x - 1, y + 1, 'S')) && characterPositionHashSet.Contains((x + 1, y + 1, 'S')))
                        || (characterPositionHashSet.Contains((x - 1, y - 1, 'S')) && characterPositionHashSet.Contains((x + 1, y - 1, 'M'))
                            && characterPositionHashSet.Contains((x - 1, y + 1, 'S')) && characterPositionHashSet.Contains((x + 1, y + 1, 'M')))
                        || (characterPositionHashSet.Contains((x - 1, y - 1, 'M')) && characterPositionHashSet.Contains((x + 1, y - 1, 'S'))
                            && characterPositionHashSet.Contains((x - 1, y + 1, 'M')) && characterPositionHashSet.Contains((x + 1, y + 1, 'S')))
                        || (characterPositionHashSet.Contains((x - 1, y - 1, 'S')) && characterPositionHashSet.Contains((x + 1, y - 1, 'S'))
                            && characterPositionHashSet.Contains((x - 1, y + 1, 'M')) && characterPositionHashSet.Contains((x + 1, y + 1, 'M')))
                        )
                    {
                        result++;
                    }
                }
            }
        }

        return result;
    }
}
