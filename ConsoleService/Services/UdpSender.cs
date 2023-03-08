using System.Net.Sockets;
using System.Text;
using ConsoleService.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ConsoleService.Services;

public class UdpSender : ApplicationBackgroundService
{
    private readonly UdpClient _client;

    public UdpSender(IHostApplicationLifetime lifetime, IOptions<UdpSettings> settings) : base(lifetime)
    {
        _client = new UdpClient(settings.Value.Ip, settings.Value.Port);
    }

    protected override async Task ExecuteOnApplicationStartAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var message = DateTimeOffset.Now.ToString();
            
            var data = Encoding.UTF8.GetBytes(message);

            await _client.SendAsync(data, stoppingToken);

            await Task.Delay(1000, stoppingToken);
        }
    }
}