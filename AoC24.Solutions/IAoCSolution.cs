namespace AoC24.Solutions;

public interface IAoCSolution<T>
{
    T GetResult();
}

public interface IAoCSolution : IAoCSolution<long>
{ }
