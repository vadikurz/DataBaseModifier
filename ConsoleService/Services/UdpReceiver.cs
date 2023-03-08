using System.Net.Sockets;
using System.Text;
using ConsoleService.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ConsoleService.Services;

public class UdpReceiver : ApplicationBackgroundService
{
    private readonly UdpClient _client;
    private readonly ILogger<UdpReceiver> _logger;

    public UdpReceiver(IHostApplicationLifetime lifetime, IOptions<UdpSettings> settings, ILogger<UdpReceiver> logger) : base(lifetime)
    {
        _client = new UdpClient(settings.Value.Port);
        _logger = logger;
    }
    
    protected override async Task ExecuteOnApplicationStartAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var result = await _client.ReceiveAsync(stoppingToken);
            
            var message = Encoding.UTF8.GetString(result.Buffer);
            
            _logger.LogInformation(message);
        }
    }
}