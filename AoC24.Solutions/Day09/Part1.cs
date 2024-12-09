namespace AoC24.Solutions.Day09;

public partial class Part1(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0901.txt";
    private readonly string _filePath = filePath;

    public Part1()
        : this(FilePath)
    { }

    public long GetResult() => Method1();

    public long Method1()
    {
        var lines = File.ReadAllLines(_filePath);

        var result = 0L;
        foreach (var line in lines)
        {
            var map = line.Select((c) => Convert.ToByte(c.ToString())).ToArray();

            var index = 0;
            var data = map.SelectMany((item, i) =>
            {
                if ((i % 2) == 0)
                {
                    var result = Enumerable.Range(0, item).Select((_) => index).ToArray();
                    index++;
                    return result;
                }
                else
                {
                    return Enumerable.Range(0, item).Select((_) => -1).ToArray();
                }
            }).ToArray();

            var offset = 0;
            var currentIndex = 0;
            var reversedStack = new Stack<int>(data);
            while (true)
            {
                if (data[currentIndex] == -1)
                {
                    int dataFromBack;
                    while ((dataFromBack = reversedStack.Pop()) == -1)
                    {
                        offset++;
                    }
                    offset++;

                    data[currentIndex] = dataFromBack;
                }
                currentIndex++;

                if (currentIndex >= reversedStack.Count)
                    break;
            }

            result += Enumerable.Range(0, currentIndex).Sum((index) => (long)index * data[index]);
        }

        return result;
    }

    private record DataBlock(byte Length, int Index, byte[] Data);
}
