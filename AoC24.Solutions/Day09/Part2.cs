namespace AoC24.Solutions.Day09;

public partial class Part2(string filePath) : IAoCSolution
{
    private static readonly string FilePath = $@"{Environment.GetEnvironmentVariable("AOC_HOME")}\Input\0901.txt";
    private readonly string _filePath = filePath;

    public Part2()
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
            var metaData = Enumerable.Range(0, map.Length)
                .Select((index) => new
                {
                    Type = index % 2, // 0 = file, 1 = free space
                    Id = index / 2, // relevant for data blocks, this is the actual data
                    Length = map[index],
                    Data = index % 2 == 0 ? Enumerable.Range(0, map[index]).Select((_) => index / 2).ToArray() : []
                })
                .ToArray();

            var dataBlocks = metaData.Where((item) => item.Type == 0).ToList();
            var data = new List<int>();
            foreach (var item in metaData)
            {
                var isData = item.Type == 0;
                var isEmpty = item.Type == 1;
                if (isData)
                {
                    if (dataBlocks.Contains(item))
                    {
                        data.AddRange(item.Data);
                        dataBlocks.Remove(item);
                    }
                    // A hole appeared here by a previous move
                    else
                    {
                        isEmpty = true;
                    }
                }

                if (isEmpty)
                {
                    var freeSpace = item.Length;
                    while (freeSpace > 0 && dataBlocks.Any((item) => item.Type == 0 && item.Length <= freeSpace))
                    {
                        for (var i = dataBlocks.Count - 1; i >= 0; i--)
                        {
                            if (dataBlocks[i].Length <= freeSpace)
                            {
                                freeSpace -= dataBlocks[i].Length;
                                data.AddRange(dataBlocks[i].Data);
                                dataBlocks.Remove(dataBlocks[i]);
                                break;
                            }
                        }
                    }
                    if (freeSpace > 0)
                        data.AddRange(Enumerable.Range(0, freeSpace).Select((_) => 0));
                }
            }

            result += Enumerable.Range(0, data.Count).Sum((index) => (long)index * data[index]);
        }

        return result;
    }
}
