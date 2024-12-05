namespace AoC24.Solutions.Day02;

public class Part2(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0201.txt";
    private readonly string _filePath = filePath;

    private const int MinIncrement = 1;
    private const int MaxIncrement = 3;

    public Part2()
        : this(FilePath)
    { }

    public long GetResult()
    {
        var data = File.ReadAllLines(_filePath)
            .Select((line) => line.Split(' ').Select((item) => Convert.ToInt32(item)).ToArray())
            .ToArray();

        var safeReports = data.Where((report) =>
        {
            var result = IsAcceptableEquidistance(report, MinIncrement, MaxIncrement);

            // Brute force approach, it took too long to fix properly
            if (!result)
            {
                for (var index = 0; index < report.Length; index++)
                {
                    result = IsAcceptableEquidistance(report.Where((item, i) => i != index).ToArray(), MinIncrement, MaxIncrement);
                    if (result)
                        break;
                }
            }

            return result;

        }).Count();

        return safeReports;
    }

    private static bool IsAcceptableEquidistance(int[] source, int minDistanceInclusive, int maxDistanceInclusive)
    {
        var levelDiff = new int[source.Length - 1];
        for (var index = 0; index < source.Length - 1; index++)
        {
            levelDiff[index] = source[index + 1] - source[index];
        }

        return (levelDiff.All((item) => item < 0) || levelDiff.All((item) => item > 0)) && levelDiff.All((item) => Math.Abs(item) >= minDistanceInclusive && Math.Abs(item) <= maxDistanceInclusive);
    }
}
