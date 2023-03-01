using Microsoft.Extensions.Hosting;

namespace ConsoleService;

public class ConsoleBackgroundService : BackgroundService
{
    private readonly IHostApplicationLifetime lifetime;
    private readonly IUdpServer server;

    public ConsoleBackgroundService(IHostApplicationLifetime lifetime, IUdpServer server)
    {
        this.lifetime = lifetime;
        this.server = server;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!await WaitForAppStartup(stoppingToken))
        {
            return;
        }
        
        while (!stoppingToken.IsCancellationRequested)
        {
            Task.Run(server.ReceiveAsync);

            await server.SendAsync();
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