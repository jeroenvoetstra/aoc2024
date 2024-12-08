using AoC24.Solutions.Day08.Models;

namespace AoC24.Solutions.Day08;

public partial class Part2(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0801.txt";
    private readonly string _filePath = filePath;

    public Part2()
        : this(FilePath)
    { }

    public long GetResult() => Method1();

    public long Method1()
    {
        var city = new City(File.ReadAllLines(_filePath));
        var grouped = city.Grid
            .SelectMany((line) => line)
            .OfType<AntennaTile>()
            .GroupBy((antenna) => antenna.Name)
            .ToList()
            ;

        var results = new List<Point>();
        foreach (var group in grouped)
        {
            results.AddRange(GetAntiNodes(group.ToList(), group.First(), city));
        }

        return results.Distinct().Count();
    }

    private static IEnumerable<Point> GetAntiNodes(IList<AntennaTile> input, AntennaTile current, City city)
    {
        var skip = input.IndexOf(current);
        foreach (var item in input.Skip(skip))
        {
            if (Equals(current, item))
                continue;

            var pointA = current.Position;
            var pointB = item.Position;

            var delta = pointB - pointA;
            for (var mirrorA = pointA; city.IsInBounds(mirrorA); mirrorA -= delta)
            {
                yield return mirrorA;
            }
            for (var mirrorB = pointB; city.IsInBounds(mirrorB); mirrorB += delta)
            {
                yield return mirrorB;
            }

            foreach (var pair in GetAntiNodes(input, item, city))
                yield return pair;
        }
    }
}
