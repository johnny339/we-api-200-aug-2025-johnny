using Marten;

namespace HelpDesk.Api.Employee.Issues;

public class VipNotificationBackgroundWorker(
    ILogger<VipNotificationBackgroundWorker> logger,
    IServiceScopeFactory scopeFactory
   ) : BackgroundService
{
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Go check if there are any unprocessed stuff in the database 
        // And THEN start listening to the channel.
        logger.LogInformation("Starting VipNotification Background Worker");

        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(TimeSpan.FromSeconds(20), stoppingToken);
            await CheckForUnhandledVips(stoppingToken);
        }
    }
    private async Task CheckForUnhandledVips(CancellationToken token)
    {
       
        using var scope = scopeFactory.CreateScope(); // "defer" in golang
        using var session = scope.ServiceProvider.GetRequiredService<IDocumentSession>();
        var problems = await session.Query<EmployeeProblemEntity>()
            .Where(p => p.Status == SubmittedIssueStatus.AwaitingTechAssignment)
            .ToListAsync(token);

        // go through each of these and if they are a VIP then do a notification.

    
        logger.LogInformation("There are {count} unhandled problems", problems.Count());

    }

   

}
