using BenchmarkDotNet.Attributes;
using System.Collections.Immutable;
using System.Text.RegularExpressions;

namespace AoC24.Solutions.Day01;

/* BENCHMARK RESULTS
// * Summary *

BenchmarkDotNet v0.14.0, Windows 10 (10.0.19045.5131/22H2/2022Update)
11th Gen Intel Core i7-1185G7 3.00GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK 8.0.404
  [Host]     : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  DefaultJob : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


| Method  | Mean     | Error    | StdDev   | Median   | Gen0     | Gen1    | Allocated |
|-------- |---------:|---------:|---------:|---------:|---------:|--------:|----------:|
| Method1 | 363.0 us |  7.26 us | 16.23 us | 360.8 us |  63.4766 | 20.0195 | 391.83 KB |
| Method2 | 672.0 us | 19.90 us | 58.37 us | 679.3 us | 129.8828 | 41.0156 |  798.1 KB |
| Method3 | 230.5 us |  5.26 us | 14.91 us | 224.9 us |  45.4102 |  4.8828 | 280.57 KB |
*/

[MemoryDiagnoser]
public class Part1 : IAoCSolution
{
    const string FilePath = @"C:\dev\personal\AoC24\Input\0101.txt";

    public long GetResult() => Method1();

    /// <summary>
    /// Basic split on space character per line, then order and calculate result.
    /// </summary>
    [Benchmark]
    public long Method1()
    {
        var pairs = File.ReadAllLines(FilePath)
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
        var pairs = File.ReadAllLines(FilePath)
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
        var fileContents = new ReadOnlySpan<byte>(File.ReadAllBytes(FilePath));
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
