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

            var dataBlocks = metaData.Where((item) => item.Type == 0).ToDictionary((item) => item.Id);
            var data = new List<int>();
            foreach (var item in metaData)
            {
                var isData = item.Type == 0;
                var isEmpty = item.Type == 1;
                if (isData)
                {
                    if (dataBlocks.ContainsKey(item.Id))
                    {
                        data.AddRange(item.Data);
                        dataBlocks.Remove(item.Id);
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
                    var keys = dataBlocks.Keys.ToList();
                    while (freeSpace > 0 && dataBlocks.Any((item) => item.Value.Type == 0 && item.Value.Length <= freeSpace))
                    {
                        for (var i = dataBlocks.Count - 1; i >= 0; i--)
                        {
                            var key = keys[i];
                            if (dataBlocks[key].Length <= freeSpace)
                            {
                                freeSpace -= dataBlocks[key].Length;
                                data.AddRange(dataBlocks[key].Data);
                                dataBlocks.Remove(dataBlocks[key].Id);
                                keys.Remove(key);
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
