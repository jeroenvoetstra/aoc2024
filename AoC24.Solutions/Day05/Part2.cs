namespace AoC24.Solutions.Day05;

public partial class Part2(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0501.txt";
    private readonly string _filePath = filePath;

    public Part2()
        : this(FilePath)
    { }

    public long GetResult()
    {
        var input = File.ReadAllText(_filePath);
        var doubleNewLine = $"{Environment.NewLine}{Environment.NewLine}";
        var sectionSplitIndex = input.IndexOf(doubleNewLine);
        var orderingRulesText = input.Substring(0, sectionSplitIndex);
        var pagesText = input.Substring(sectionSplitIndex + doubleNewLine.Length);

        var rules = orderingRulesText.Split(Environment.NewLine).Select((rule) => rule.Split('|').Select((number) => Convert.ToInt32(number)).ToArray()).ToArray();
        var pagesLines = pagesText.Split(Environment.NewLine).Select((line) => line.Split(',').Select((number) => Convert.ToInt32(number)).ToArray()).ToArray();

        var result = 0L;
        foreach (var pagesLine in pagesLines)
        {
            var valid = true;
            for (var i = 0; i < pagesLine.Length - 1; i++)
            {
                if (!rules.Any((rule) => rule[0] == pagesLine[i] && rule[1] == pagesLine[i + 1]))
                {
                    valid = false;
                    break;
                }
            }

            if (!valid)
            {
                var relevantRules = rules.Where((rule) => pagesLine.Contains(rule[0]) && pagesLine.Contains(rule[1]));
                var orderedRules = relevantRules.GroupBy((rule) => rule[0], (rule) => rule[1]).OrderByDescending((item) => item.Count()).ToDictionary((item) => item.Key, (item) => item.ToArray());
                // Ordered by count means we now have the keys in the correct order, it will look like this:
                // { xx: [xx comes before 4 other pages] }
                // { xx: [xx comes before 3 other pages] }
                // { xx: [xx comes before 2 other pages] }
                // { xx: [xx comes before 1 other pages] }
                // { (missing): null } --> 0 pages come before it
                // Reverse order is used so we can append the null result (which should be the first page as nothing comes before it) in order
                // to be able to divide by half of the length and get the middle result.
                //
                // BTW this is the defensive ordered map that I talked about in part 1.

                result += orderedRules.Keys.ToArray()[(orderedRules.Keys.Count + 1) / 2];
            }
        }

        return result;
    }
}
