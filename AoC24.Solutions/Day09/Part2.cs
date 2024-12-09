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
                    Data = index % 2 == 0 ? Enumerable.Range(0, map[index]).Select((_) => (short)(index / 2)).ToArray() : []
                })
                .ToArray();

            
        }

        return result;
    }
}
