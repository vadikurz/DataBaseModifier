using ConsoleService;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddHostedService<ConsoleBackgroundService>();
                services.AddSingleton<IUdpServer>(provider => new UdpServer("127.0.0.1", 22220));
            })
            .Build();

        await host.RunAsync();
    }
}