using Microsoft.Extensions.Hosting;

namespace ConsoleService.Services;

public abstract class ApplicationBackgroundService : BackgroundService
{
    private readonly IHostApplicationLifetime _lifetime;
    
    protected ApplicationBackgroundService(IHostApplicationLifetime lifetime)
    {
        _lifetime = lifetime;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!await WaitForAppStartup(stoppingToken))
        {
            return;
        }

        await ExecuteOnApplicationStartAsync(stoppingToken);
    }

    protected abstract Task ExecuteOnApplicationStartAsync(CancellationToken stoppingToken);

    private async Task<bool> WaitForAppStartup(CancellationToken stoppingToken)
    {
        var startedSource = new TaskCompletionSource();
        var cancelledSource = new TaskCompletionSource();

        await using var reg1 = _lifetime.ApplicationStarted.Register(() => startedSource.SetResult());
        await using var reg2 = stoppingToken.Register(() => cancelledSource.SetResult());

        var completedTask = await Task.WhenAny(
            startedSource.Task,
            cancelledSource.Task);
        
        return completedTask == startedSource.Task;
    }
}