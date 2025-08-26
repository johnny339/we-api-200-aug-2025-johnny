
using System.Threading.Tasks;
using Marten;
using Marten.Linq.Parsing.Operators;

namespace HelpDesk.Api.Employee.Issues;

public class VipNotificationBackgroundWorker(
    ILogger<VipNotificationBackgroundWorker> logger,
    IServiceScopeFactory scopeFactory
   ) : IHostedService, IDisposable
{
    private int _count = 0;
    private PeriodicTimer? _timer = null;
    public void Dispose()
    {
       _timer?.Dispose();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting VipNotification Background Worker");
        _timer = new PeriodicTimer(TimeSpan.FromSeconds(20));
        while (await _timer.WaitForNextTickAsync(cancellationToken))
        {
            await CheckForUnhandledVips();
        }
       
    }

    private async Task CheckForUnhandledVips()
    {
        var count = Interlocked.Increment(ref _count);
        using var scope = scopeFactory.CreateScope(); // "defer" in golang
        using var session = scope.ServiceProvider.GetRequiredService<IDocumentSession>();
        var problems = await session.Query<EmployeeProblemEntity>()
            .Where(p => p.Status == SubmittedIssueStatus.AwaitingTechAssignment)
            .ToListAsync();


        logger.LogInformation("Checking for unhandled VIPs {count}", _count);
        logger.LogInformation("There are {count} unhandled problems", problems.Count());

    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Shutting down the background vip checker thing");
        
        return Task.CompletedTask;
    }
}
