namespace AoC24.Solutions;

/// <summary>
/// :P
/// </summary>
public class AoCSolutionFactory
{
    public static IAoCSolution<TResult> Create<TSolution, TResult>()
        where TSolution : IAoCSolution<TResult>, new()
    {
        return new TSolution();
    }

    public static TResult GetResult<TSolution, TResult>()
        where TSolution : IAoCSolution<TResult>, new()
    {
        return Create<TSolution, TResult>().GetResult();
    }

    public static IAoCSolution<long> Create<TSolution>()
        where TSolution : IAoCSolution<long>, new()
    {
        return new TSolution();
    }

    public static long GetResult<TSolution>()
        where TSolution : IAoCSolution<long>, new()
    {
        return Create<TSolution, long>().GetResult();
    }
}
