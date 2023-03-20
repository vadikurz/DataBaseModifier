using System.Net.Sockets;
using ConsoleService.Models;
using ConsoleService.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ConsoleService.Services;

public class UdpSender : ApplicationBackgroundService
{
    private readonly UdpSenderSettings _udpSenderSettings; 

    public UdpSender(IHostApplicationLifetime lifetime, IOptions<UdpSenderSettings> udpSenderSettings) : base(lifetime)
    {
        _udpSenderSettings = udpSenderSettings.Value;
    }

    protected override async Task ExecuteInternalAsync(CancellationToken stoppingToken)
    {
        try
        {
            using var client = new UdpClient(_udpSenderSettings.Ip, _udpSenderSettings.Port);

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
        catch (OperationCanceledException exception)
        {
            
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
        }
    }

    private List<Point> GetPoints()
    {
        var points = new List<Point>();

        using var reader = new StringReader(TestData.Points);

        while (reader.ReadLine() is {} line)
        {
            if (Point.TryParse(line, out var point))
                points.Add(point!);
        }
        
        return points;
    }
}
