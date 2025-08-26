


namespace Demos.Api.Home;

public class HitCounter : ICountHits
{
    private int _hitCount;

    //private object _hitLock = new object();



    public async Task<int> GetHitCount(CancellationToken token)
    {
        var count = Interlocked.Increment(ref _hitCount);
        return count;
    }
}
