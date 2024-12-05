namespace AoC24.Solutions.Day05;

public partial class Part1 : IAoCSolution
{
    static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0501.txt";

    public long GetResult()
    {
        var input = File.ReadAllText(FilePath);
        var doubleNewLine = $"{Environment.NewLine}{Environment.NewLine}";
        var sectionSplitIndex = input.IndexOf(doubleNewLine);
        var orderingRulesText = input.Substring(0, sectionSplitIndex);
        var pagesText = input.Substring(sectionSplitIndex + doubleNewLine.Length);

        var rules = orderingRulesText.Split(Environment.NewLine).Select((rule) => rule.Split('|').Select((number) => Convert.ToInt32(number)).ToArray()).ToArray();
        var pagesLines = pagesText.Split(Environment.NewLine).Select((line) => line.Split(',').Select((number) => Convert.ToInt32(number)).ToArray()).ToArray();

        /*
         * Funnily enough we cannot use the logic that is usable in the example, which is to index the pages with arrays of pages that come before it
         * and sorting by the size of these arrays. In a way that I still don't understand, every single one of the 49 different pages come before 24 others
         * with no cycles. There should at least be some pages (in the upper half) that have less than 24 pages after it, but this is not the case. All pages
         * that are used are also indexed (i.e. there are 49 different pages, both in the rules and in print instructions), so I'm a little confused.
         * 
         * TLDR; tried to build a map of the order defensively but couldn't. No choice but to search for the individual combinations.
         */

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

            if (valid)
            {
                result += pagesLine[pagesLine.Length / 2];
            }
        }

        return result;
    }
}
