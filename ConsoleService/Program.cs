using Common;
using ConsoleService.Services;
using ConsoleService.Settings;

namespace ConsoleService;

public class Program
{
    public static async Task Main(string[] args)
    {
        await CreateHostBuilder(args)
            .Build()
            .RunAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        return Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.Configure<UdpReceiverSettings>(configuration.GetSection("UdpReceiverSettings"));
                services.AddSingleton<LbsService>();
                services.AddSingleton<WaitingForAppStartupService>();
                services.AddHostedService<UdpReceiver>();
            });
    }
}