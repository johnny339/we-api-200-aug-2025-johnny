
namespace Demos.Api.Home
{
    public interface ICountHits
    {
        Task<int> GetHitCount(CancellationToken token);
    }
}