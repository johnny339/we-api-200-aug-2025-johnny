

using Marten;

namespace Demos.Api.Home;

public class PersistentHitCounter(
    Guid id,
    IServiceScopeFactory scopeFactory
    ) : ICountHits

{
    
    public async Task<int> GetHitCount(CancellationToken token)
    {
        // inside a singleton, but I need to use a "scoped" service - 
        using var scope = scopeFactory.CreateScope();
        using var session = scope.ServiceProvider.GetRequiredService<IDocumentSession>();
        var count = 0;
            
        var hitCount = await session.Query<HitCounterEntity>()
            .Where(h => h.Id == id)
            .SingleOrDefaultAsync(token);
    
        if (hitCount is null)
        {
            session.Store(new HitCounterEntity(id, 1));
            count = 1;
        }
        else
        {
            var updatedHitCount = hitCount with { Count = hitCount.Count + 1 };
            session.Store(updatedHitCount);
           count = updatedHitCount.Count;
        }
        await session.SaveChangesAsync(token);
        return count;
    }
}

public record HitCounterEntity(Guid Id, int Count);