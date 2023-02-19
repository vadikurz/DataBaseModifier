using ConsoleService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services => services.AddHostedService<ConsoleBackgroundService>()).Build();
        
        await host.RunAsync();
    }
}