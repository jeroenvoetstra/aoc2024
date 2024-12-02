namespace AoC24.Solutions.Day02;

public class Part1 : IAoCSolution
{
    static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0201.txt";

    public long GetResult()
    {
        var data = File.ReadAllLines(FilePath)
            .Select((line) => line.Split(' ').Select((item) => Convert.ToInt32(item)).ToArray())
            .ToArray();

        var safeReports = data.Where((report) =>
        {
            var levelDiff = new int[report.Length - 1];
            for (var index = 0; index < report.Length - 1; index++)
            {
                levelDiff[index] = report[index + 1] - report[index];
            }
            return (levelDiff.All((item) => item < 0) || levelDiff.All((item) => item > 0)) && levelDiff.All((item) => Math.Abs(item) >= 1 && Math.Abs(item) <= 3);
        }).Count();

        return safeReports;
    }
}
