using System.Net.Sockets;
using System.Text;
using ConsoleService.Models;
using ConsoleService.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ConsoleService.Services;

public class UdpReceiver : ApplicationBackgroundService
{
    private readonly ILogger<UdpReceiver> _logger;
    private readonly LbsService _lbsService;
    private readonly int _port;

    public UdpReceiver(IHostApplicationLifetime lifetime, IOptions<UdpReceiverSettings> settings, ILogger<UdpReceiver> logger, LbsService lbsService) : base(lifetime)
    {
        _logger = logger;
        _port = settings.Value.Port;
        _lbsService = lbsService;
    }
    
    protected override async Task ExecuteInternalAsync(CancellationToken stoppingToken)
    {
        try
        {
            using var client = new UdpClient(_port);
        
            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await client.ReceiveAsync(stoppingToken);
            
                var message = Encoding.UTF8.GetString(result.Buffer);

                if (Point.TryParse(message, out var point))
                    _logger.LogInformation(_lbsService.GetCoordinates(point!).ToString());
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}