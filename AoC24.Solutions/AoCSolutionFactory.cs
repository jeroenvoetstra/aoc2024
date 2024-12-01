namespace AoC24.Solutions;

/// <summary>
/// :P
/// </summary>
public class AoCSolutionFactory
{
    public static IAoCSolution Create<TSolution>()
        where TSolution : IAoCSolution, new()
    {
        return new TSolution();
    }

    public static long GetResult<TSolution>()
        where TSolution : IAoCSolution, new()
    {
        return Create<TSolution>().GetResult();
    }
}
