using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Models;
using ConsoleService.Integration.Tests.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ConsoleService.Integration.Tests;

public class UdpSender : BackgroundService
{
    private readonly TestsSettings _testsSettings;
    private readonly WaitingForAppStartupService _waitingUpService;

    private readonly Lazy<List<Point>> _lazy = new(() =>
    {
        var points = new List<Point>();

        using var reader = new StringReader(TestData.Points);

        while (reader.ReadLine() is { } line)
        {
            if (Point.TryParse(line, out var point))
                points.Add(point!);
        }

        return points;
    });

    private List<Point> Points => _lazy.Value;

    public UdpSender(WaitingForAppStartupService waitingUpService, IOptions<TestsSettings> udpSenderSettings)
    {
        _testsSettings = udpSenderSettings.Value;
        _waitingUpService = waitingUpService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        try
        {
            if (!await _waitingUpService.WaitForAppStartup(stoppingToken))
            {
                return;
            }

            using var client = new UdpClient(_testsSettings.Ip, _testsSettings.SendingPort);

            while (!stoppingToken.IsCancellationRequested)
            {
                for (var i = 0; i < Points.Count; i++)
                {
                    if (i > 0)
                    {
                        var delay = Points[i].Time - Points[i - 1].Time;
                        await Task.Delay(delay, stoppingToken);
                    }

                    await client.SendAsync(Encoding.UTF8.GetBytes(Points[i].ToString()), stoppingToken);
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