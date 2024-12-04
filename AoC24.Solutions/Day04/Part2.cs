using static AoC24.Solutions.Day04.Part1;

namespace AoC24.Solutions.Day04;

public partial class Part2 : IAoCSolution
{
    static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0401.txt";

    public long GetResult()
    {
        var characterMatrix = File.ReadAllLines(FilePath).ToArray();
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
}
