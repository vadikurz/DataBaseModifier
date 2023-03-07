using Microsoft.Extensions.Hosting;

namespace ConsoleService;

public class ConsoleBackgroundService : BackgroundService
{
    private readonly IHostApplicationLifetime lifetime;
    private readonly IUdpSender sender;
    private readonly IUdpReceiver receiver;

    public ConsoleBackgroundService(IHostApplicationLifetime lifetime, IUdpSender sender, IUdpReceiver receiver)
    {
        this.lifetime = lifetime;
        this.sender = sender;
        this.receiver = receiver;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!await WaitForAppStartup(stoppingToken))
        {
            return;
        }
        
        while (!stoppingToken.IsCancellationRequested)
        {
            await sender.SendAsync(DateTimeOffset.Now.ToString());
            await receiver.ReceiveAsync();
            
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