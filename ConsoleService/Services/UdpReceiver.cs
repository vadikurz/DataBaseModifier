using System.Net.Sockets;
using System.Text;
using ConsoleService.Models;
using ConsoleService.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ConsoleService.Services;

public class UdpReceiver : BackgroundService
{
    private readonly ILogger<UdpReceiver> _logger;
    private readonly LbsService _lbsService;
    private readonly UdpReceiverSettings _settings;
    private readonly WaitingForAppStartupService _upService;

    public UdpReceiver(WaitingForAppStartupService upService, IOptions<UdpReceiverSettings> settings, ILogger<UdpReceiver> logger, LbsService lbsService)
    {
        _logger = logger;
        _settings = settings.Value;
        _lbsService = lbsService;
        _upService = upService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            if (!await _upService.WaitForAppStartup(stoppingToken))
            {
                return;
            }
            
            using var client = new UdpClient(_settings.Port);

            while (!stoppingToken.IsCancellationRequested)
            {
                var result = await client.ReceiveAsync(stoppingToken);

                var message = Encoding.UTF8.GetString(result.Buffer);

                if (Point.TryParse(message, out var point))
                {
                    if (_lbsService.TryGetCoordinates(point!, out var coords))
                    {
                        _logger.LogInformation(coords.ToString());
                    }
                }
            }
        }
        catch (OperationCanceledException exception)
        {
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }
}