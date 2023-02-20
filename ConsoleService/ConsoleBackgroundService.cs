using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsoleService;

public class ConsoleBackgroundService : BackgroundService
{
    private readonly ILogger<ConsoleBackgroundService> logger;
    private readonly IHostApplicationLifetime lifetime;

    public ConsoleBackgroundService(ILogger<ConsoleBackgroundService> logger, IHostApplicationLifetime lifeTime)
    {
        this.logger = logger;
        this.lifetime = lifeTime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!await WaitForAppStartup(stoppingToken))
        {
            return;
        }

        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("task running...");
            await Task.Delay(1000, stoppingToken);
        }
    }
    
    private async Task<bool> WaitForAppStartup(CancellationToken stoppingToken)
    {
        var startedSource = new TaskCompletionSource();
        var cancelledSource = new TaskCompletionSource();

        await using var reg1 = lifetime.ApplicationStarted.Register(() => startedSource.SetResult());
        await using var reg2 = stoppingToken.Register(() => cancelledSource.SetResult());

        Console.WriteLine(Environment.CurrentManagedThreadId);
        
        var completedTask = await Task.WhenAny(
            startedSource.Task,
            cancelledSource.Task);
        
        return completedTask == startedSource.Task;
    }
}