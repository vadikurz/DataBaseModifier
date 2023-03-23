using ConsoleService.Services;
using ConsoleService.Settings;
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
                services.Configure<UdpSenderSettings>(configuration.GetSection("UdpSenderSettings"));
                services.Configure<UdpReceiverSettings>(configuration.GetSection("UdpReceiverSettings"));
                services.AddSingleton<LbsService>();
                services.AddSingleton<WaitingForAppStartupService>();
                services.AddHostedService<UdpSender>();
                services.AddHostedService<UdpReceiver>();
            })
            .Build();

        await host.RunAsync();
    }
}