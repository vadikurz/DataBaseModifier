using System.Net.Sockets;
using ConsoleService.Models;
using ConsoleService.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ConsoleService.Services;

public class UdpSender : ApplicationBackgroundService
{
    private readonly string _ip;
    private readonly int _port;

    public UdpSender(IHostApplicationLifetime lifetime, IOptions<UdpSenderSettings> udpSenderSettings) : base(lifetime)
    {
        _ip = udpSenderSettings.Value.Ip;
        _port = udpSenderSettings.Value.Port;

    }

    protected override async Task ExecuteInternalAsync(CancellationToken stoppingToken)
    {
        try
        {
            using var client = new UdpClient(_ip, _port);
        
            var points = GetPoints();

            while (!stoppingToken.IsCancellationRequested)
            {
                for (var i = 0; i < points.Count; i++)
                {
                    if (i > 0)
                    {
                        var delay = points[i].Time - points[i - 1].Time;
                        await Task.Delay(delay, stoppingToken);
                    }
                
                    await client.SendAsync(points[i].Serialize(), stoppingToken);
                }
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    private List<Point> GetPoints()
    {
        var pointsArray = TestData.Points.Split("\r\n");
        
        var points = new List<Point>(pointsArray.Length);

        foreach (var point in pointsArray)
        {
            if (Point.TryParse(point, out var resultPoint))
                points.Add(resultPoint!);
        }
        
        return points;
    }
}
