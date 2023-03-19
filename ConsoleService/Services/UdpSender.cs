using System.Net.Sockets;
using ConsoleService.Models;
using ConsoleService.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ConsoleService.Services;

public class UdpSender : ApplicationBackgroundService
{
    private readonly UdpClient _client;
    private readonly string _pathToObjectPoints;

    public UdpSender(IHostApplicationLifetime lifetime, IOptions<UdpSettings> udpSettings,
        IOptions<DataSourceSettings> dataSourceSettings) : base(lifetime)
    {
        _client = new UdpClient(udpSettings.Value.Ip, udpSettings.Value.Port);
        _pathToObjectPoints = dataSourceSettings.Value.ObjectCoordinatesFilePath;
    }

    protected override async Task ExecuteOnApplicationStartAsync(CancellationToken stoppingToken)
    {
        var objectPoints = await GetListOfObjectPoints(_pathToObjectPoints);

        while (!stoppingToken.IsCancellationRequested)
        {
            for (var i = 0; i < objectPoints.Count; i++)
            {
                var data = objectPoints[i].Serialize();

                await _client.SendAsync(data, stoppingToken);

                if (i == objectPoints.Count - 1) continue;

                var timeBetweenPoints = objectPoints[i + 1].TimeStamp.Subtract(objectPoints[i].TimeStamp);

                await Task.Delay(timeBetweenPoints, stoppingToken);
            }
        }
    }

    private async Task<List<Point>> GetListOfObjectPoints(string pathToObjectCoordinates)
    {
        var listOfObjectCoordinates = new List<Point>();

        var streamReader = File.OpenText(pathToObjectCoordinates);

        while (await streamReader.ReadLineAsync() is { } line)
        {
            if (Point.TryParse(line, out var point))
                listOfObjectCoordinates.Add(point);
        }

        return listOfObjectCoordinates;
    }
}