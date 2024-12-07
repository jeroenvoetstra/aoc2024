namespace AoC24.Solutions.Day07;

public partial class Part1(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0701.txt";
    private readonly string _filePath = filePath;

    public Part1()
        : this(FilePath)
    { }

    public long GetResult() => Method1();

    public long Method1()
    {
        var lines = File.ReadLines(_filePath);
        var data = lines.Select(
            (line) => line.Split(':') is string[] equation ?
                new
                {
                    Answer = Convert.ToInt64(equation[0].Trim()),
                    Components = equation[1].Trim().Split(' ').Select((component) => Convert.ToInt64(component.Trim())).ToArray()
                } : null
                );

        var totalResult = 0L;
        foreach (var equation in data)
        {
            if (AddOrMultiply(equation!.Components, 0, equation.Components[0]).Any((result) => result == equation.Answer))
                totalResult += equation.Answer;
        }

        return totalResult;
    }

    public IEnumerable<long> AddOrMultiply(long[] array, int index, long currentResult)
    {
        if (index == array.Length - 1)
        {
            yield return currentResult;
            yield break;
        }

        var nextIndex = index + 1;
        foreach (var result in AddOrMultiply(array, nextIndex, currentResult + array[nextIndex]))
        {
            yield return result;
        }
        foreach (var result in AddOrMultiply(array, nextIndex, currentResult * array[nextIndex]))
        {
            yield return result;
        }
    }

}
