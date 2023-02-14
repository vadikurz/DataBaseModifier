using Microsoft.Extensions.Hosting;

namespace ConsoleService;

public class ConsoleBackgroundService : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            await Task.Delay(1000, stoppingToken);
            Console.WriteLine("task running...");
            
        }
    }
}