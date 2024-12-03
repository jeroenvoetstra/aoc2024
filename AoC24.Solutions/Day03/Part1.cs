using BenchmarkDotNet.Attributes;
using System.Text.RegularExpressions;

namespace AoC24.Solutions.Day03;

/*
// * Summary *

BenchmarkDotNet v0.14.0, Windows 10 (10.0.19045.5131/22H2/2022Update)
11th Gen Intel Core i7-1185G7 3.00GHz, 1 CPU, 8 logical and 4 physical cores
.NET SDK 9.0.100
  [Host]   : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 8.0 : .NET 8.0.11 (8.0.1124.51707), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI
  .NET 9.0 : .NET 9.0.0 (9.0.24.52809), X64 RyuJIT AVX-512F+CD+BW+DQ+VL+VBMI


| Method                          | Job      | Runtime  | Mean       | Error    | StdDev   | Median     | Gen0     | Gen1     | Allocated  |
|-------------------------------- |--------- |--------- |-----------:|---------:|---------:|-----------:|---------:|---------:|-----------:|
| GetMuls                         | .NET 8.0 | .NET 8.0 |   455.3 us | 14.78 us | 41.70 us |   439.1 us |  99.6094 |  56.6406 |  616.09 KB |
| AllOperationsWithGeneratedRegex | .NET 8.0 | .NET 8.0 | 1,697.4 us | 33.51 us | 50.16 us | 1,682.4 us | 343.7500 | 308.5938 | 2111.59 KB |
| AllOperationsWithRegularRegex   | .NET 8.0 | .NET 8.0 | 2,116.2 us | 40.43 us | 48.13 us | 2,099.9 us | 343.7500 | 304.6875 |  2111.6 KB |
| GetMuls                         | .NET 9.0 | .NET 9.0 |   406.3 us |  5.74 us |  5.09 us |   405.9 us |  99.6094 |  52.7344 |  616.09 KB |
| AllOperationsWithGeneratedRegex | .NET 9.0 | .NET 9.0 | 1,584.6 us | 31.33 us | 58.85 us | 1,565.6 us | 332.0313 | 304.6875 | 2049.77 KB |
| AllOperationsWithRegularRegex   | .NET 9.0 | .NET 9.0 | 2,093.1 us | 33.83 us | 29.99 us | 2,085.0 us | 328.1250 | 320.3125 | 2049.77 KB |
*/

[MemoryDiagnoser]
[SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net80)]
[SimpleJob(BenchmarkDotNet.Jobs.RuntimeMoniker.Net90)]
public partial class Part1 : IAoCSolution
{
    static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0301.txt";

    private static readonly Regex MultiplyPattern = new Regex(@"((mul\((?<num1>\d+)\,(?<num2>\d+)\)))");

    public long GetResult() => AllOperationsWithRegularRegex();

    /// <summary>
    /// Original method only retrieving what we're interested in.
    /// </summary>
    /// <returns></returns>
    [Benchmark]
    public long GetMuls()
    {
        var content = File.ReadAllText(FilePath);
        var matches = MultiplyPattern.Matches(content);

        var result = 0L;
        foreach (Match match in matches)
        {
            var num1 = Convert.ToInt32(match.Groups["num1"].Value);
            var num2 = Convert.ToInt32(match.Groups["num2"].Value);
            result += num1 * num2;

        }

        return result;
    }

    [GeneratedRegex(@"(?<operation>[\w\']+)\((((?<args>\d+)\,?)*\))", RegexOptions.IgnoreCase)]
    private static partial Regex OperationPatternGenerated();

    /// <summary>
    /// Method interpreting all instructions from the input, using a generated regex.
    /// </summary>
    [Benchmark]
    public long AllOperationsWithGeneratedRegex()
    {
        var content = File.ReadAllText(FilePath);
        var operations = new List<Operation>();

        var matches = OperationPatternGenerated().Matches(content);
        foreach (Match match in matches)
        {
            var op = match.Groups["operation"]?.Value?.Replace("'", "");
            if (!Operation.OperationTypes.Contains(op))
                continue;

            var args = new string[0];
            if (match.Groups["args"] != null)
            {
                args = match.Groups["args"].Captures.OfType<Capture>().Select((capture) => capture.Value).ToArray();
            }

            operations.Add(new()
            {
                Name = op!,
                Args = args.Select((item) => Convert.ToInt32(item))?.ToArray()
            });
        }

        return operations.Where((op) => op.Name == "mul" && op.Args!.Length > 1).Sum((op) => op.Args!.Aggregate((left, right) => left * right));
    }

    private static readonly Regex OperationPattern = new Regex(@"(?<operation>[\w\']+)\((((?<args>\d+)\,?)*\))");

    /// <summary>
    /// Method interpreting all instructions from the input, using a regular regex.
    /// </summary>
    [Benchmark]
    public long AllOperationsWithRegularRegex()
    {
        var content = File.ReadAllText(FilePath);
        var operations = new List<Operation>();

        var matches = OperationPattern.Matches(content);
        foreach (Match match in matches)
        {
            var op = match.Groups["operation"]?.Value?.Replace("'", "");
            if (!Operation.OperationTypes.Contains(op))
                continue;

            var args = new string[0];
            if (match.Groups["args"] != null)
            {
                args = match.Groups["args"].Captures.OfType<Capture>().Select((capture) => capture.Value).ToArray();
            }

            operations.Add(new()
            {
                Name = op!,
                Args = args.Select((item) => Convert.ToInt32(item))?.ToArray()
            });
        }

        return operations.Where((op) => op.Name == "mul" && op.Args!.Length > 1).Sum((op) => op.Args!.Aggregate((left, right) => left * right));
    }

    private class Operation
    {
        public static readonly string[] OperationTypes = ["mul", "do", "dont", "select", "why", "when", "what", "from", "who", "how", "where"];

        public required string Name { get; set; }
        public int[]? Args { get; set; }

        public override string ToString()
        {
            return $"OPERATION={Name}, ARGS={(Args != null ? string.Join(" | ", Args) : null)}";
        }
    }
}
