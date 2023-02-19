using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsoleService;

public class ConsoleBackgroundService : BackgroundService
{
    private ILogger<ConsoleBackgroundService> logger;

    public ConsoleBackgroundService(ILogger<ConsoleBackgroundService> logger)
    {
        this.logger = logger;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            logger.LogInformation("task running...");
            await Task.Delay(1000, stoppingToken);
        }
    }
}