using BenchmarkDotNet.Attributes;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace AoC24.Solutions.Day01;

/* BENCHMARK RESULTS
// * Summary *

BenchmarkDotNet v0.14.0, Windows 10 (10.0.19045.5131/22H2/2022Update)
11th Gen Intel Core i7-1185G7 3.00GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK 9.0.100
  [Host]   : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0 : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 9.0 : .NET 9.0.0 (9.0.24.52809), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


| Method  | Job      | Runtime  | Mean     | Error   | StdDev  | Gen0     | Gen1    | Allocated |
|-------- |--------- |--------- |---------:|--------:|--------:|---------:|--------:|----------:|
| Method1 | .NET 8.0 | .NET 8.0 | 351.6 us | 6.27 us | 5.56 us |  63.4766 | 20.0195 | 391.83 KB |
| Method2 | .NET 8.0 | .NET 8.0 | 586.2 us | 9.85 us | 8.74 us | 128.9063 | 41.0156 |  798.1 KB |
| Method3 | .NET 8.0 | .NET 8.0 | 208.7 us | 4.17 us | 4.96 us |  45.6543 |  4.8828 | 280.57 KB |
| Method1 | .NET 9.0 | .NET 9.0 | 296.2 us | 4.82 us | 6.91 us |  56.1523 | 15.1367 | 344.62 KB |
| Method2 | .NET 9.0 | .NET 9.0 | 547.8 us | 7.71 us | 7.92 us | 128.9063 | 33.2031 | 789.95 KB |
| Method3 | .NET 9.0 | .NET 9.0 | 193.4 us | 3.85 us | 4.12 us |  45.6543 |  4.8828 | 280.62 KB |
*/

[MemoryDiagnoser]
[SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net80)]
[SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net90)]
public class Part1(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0101.txt";
    private readonly string _filePath = filePath;

    public Part1()
        : this(FilePath)
    { }

    public long GetResult() => Method3();

    /// <summary>
    /// Basic split on space character per line, then order and calculate result.
    /// </summary>
    [Benchmark]
    public long Method1()
    {
        var pairs = File.ReadAllLines(_filePath)
            .Where((line) => !string.IsNullOrWhiteSpace(line))
            .Select((line) => line.Split(' ').Where((item) => !string.IsNullOrEmpty(item)).Select((item) => Convert.ToInt32(item.Trim())).ToArray())
            .ToArray();
        var left = Enumerable.Range(0, pairs.Length).Select((index) => pairs[index][0]).Order().ToArray();
        var right = Enumerable.Range(0, pairs.Length).Select((index) => pairs[index][1]).Order().ToArray();

        var solution = 0;
        for (var index = 0; index < pairs.Length; index++)
        {
            solution += Math.Abs(right[index] - left[index]);
        }

        return solution;
    }

    /// <summary>
    /// Uses Regex to extract the numbers from each line, then order and calculate result.
    /// </summary>
    [Benchmark]
    public long Method2()
    {
        var pairs = File.ReadAllLines(_filePath)
            .Where((line) => !string.IsNullOrWhiteSpace(line))
            .Select((line) => Regex.Matches(line, @"\d+").Select((match) => Convert.ToInt32(match.Value)).ToArray())
            .ToArray();

        var left = Enumerable.Range(0, pairs.Length).Select((index) => pairs[index][0]).Order().ToImmutableArray();
        var right = Enumerable.Range(0, pairs.Length).Select((index) => pairs[index][1]).Order().ToImmutableArray();

        var solution = 0;
        for (var index = 0; index < pairs.Length; index++)
        {
            solution += Math.Abs(right[index] - left[index]);
        }

        return solution;
    }

    /// <summary>
    /// Optimized method, uses byte buffers to get numbers back (ASCII), can probably be optimized further.
    /// The conversion to chars to strings to ints could be subject of optimization, as well as the ordering.
    /// </summary>
    [Benchmark]
    public long Method3()
    {
        var fileContents = new ReadOnlySpan<byte>(File.ReadAllBytes(_filePath));
        // count lines
        var lineCount = fileContents.Count<byte>(0x0A);
        // if there's no line feed, add 1
        lineCount += fileContents[^1] == 0x0A ? 0 : 1;
        // setup left and right data buffers
        var data = new int[2][];
        data[0] = new int[lineCount];
        data[1] = new int[lineCount];
        // setup read buffers and tracking
        var buffer = new char[20];
        var bufferIndex = 0;
        var lineCounter = 0;
        var sideCounter = 0;
        foreach (var b in fileContents)
        {
            // newline means we read and empty buffer, switch sides and go to the next line
            if (b == 0x0A)
            {
                data[sideCounter][lineCounter] = Convert.ToInt32(new string(buffer));
                sideCounter = sideCounter == 0 ? 1 : 0;
                lineCounter++;
                bufferIndex = 0;
                buffer = new char[20];
            }
            // space means we can read and empty buffer, and switch sides
            else if ((char)b == ' ')
            {
                // if we already cleared the buffer, this is a space that follows a space which means we can skip
                if (buffer[0] == 0)
                    continue;

                data[sideCounter][lineCounter] = Convert.ToInt32(new string(buffer));
                sideCounter = sideCounter == 0 ? 1 : 0;
                bufferIndex = 0;
                buffer = new char[20];
            }
            // all that is not control char (we will only encounter \r and \n here) will be put in the buffer
            else if ((char)b != '\r')
            {
                buffer[bufferIndex++] = (char)b;
            }
        }
        // if there's still something in buffer, do one last read
        if (buffer[0] != 0)
        {
            data[sideCounter][lineCounter] = Convert.ToInt32(new string(buffer));
        }

        // order
        data[0] = data[0].Order().ToArray();
        data[1] = data[1].Order().ToArray();

        var solution = 0;
        for (var index = 0; index < lineCount; index++)
        {
            solution += Math.Abs(data[1][index] - data[0][index]);
        }

        return solution;
    }
}
