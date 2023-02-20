using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsoleService;

public class ConsoleBackgroundService : BackgroundService
{
    private ILogger<ConsoleBackgroundService> logger;
    private bool ready;

    public ConsoleBackgroundService(ILogger<ConsoleBackgroundService> logger, IHostApplicationLifetime lifeTime)
    {
        this.logger = logger;
        lifeTime.ApplicationStarted.Register(() => ready = true);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!ready)
        {
            await Task.Delay(1000, stoppingToken);
        }
        
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("task running...");
            await Task.Delay(1000, stoppingToken);
        }
    }
    
    private async Task<bool> WaitForAppStartup(IHostApplicationLifetime appLifetime, CancellationToken stoppingToken)
    {
        var startedSource = new TaskCompletionSource();
        var cancelledSource = new TaskCompletionSource();

        await using var reg1 = appLifetime.ApplicationStarted.Register(() => startedSource.SetResult());
        await using var reg2 = stoppingToken.Register(() => cancelledSource.SetResult());

        var completedTask = await Task.WhenAny(
            startedSource.Task,
            cancelledSource.Task).ConfigureAwait(false);
        
        return completedTask == startedSource.Task;
    }
}