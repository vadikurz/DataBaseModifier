using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConsoleService;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.AddHostedService<ConsoleBackgroundService>();
                services.AddSingleton<IUdpSender>(provider => new UdpSender("127.0.0.1", 22220));
                services.AddSingleton<IUdpReceiver>(provider => new UdpReceiver(22220));
            })
            .Build();

        await host.RunAsync();
    }
}