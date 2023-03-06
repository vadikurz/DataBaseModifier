using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ConsoleService;

public class Program
{
    public static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var host = Host.CreateDefaultBuilder()
            .ConfigureServices(services =>
            {
                services.Configure<UdpSettings>(configuration.GetSection("UdpSettings"));
                services.AddHostedService<ConsoleBackgroundService>();
                services.AddSingleton<IUdpSender, UdpSender>();
                services.AddSingleton<IUdpReceiver, UdpReceiver>();
            })
            .Build();

        await host.RunAsync();
    }
}