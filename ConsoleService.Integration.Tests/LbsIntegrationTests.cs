using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Models;
using ConsoleService.Integration.Tests.Settings;
using WorkerService.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using static ConsoleService.Integration.Tests.TestData;

namespace ConsoleService.Integration.Tests;

public class LbsIntegrationTests
{
    private static WorkerApplicationFactory<Program> _workerApplicationFactory;

    [SetUp]
    public static async Task ClassInit()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        
        _workerApplicationFactory = new WorkerApplicationFactory<Program>()
            .WithHostBuilder(builder =>
            {
                builder.ConfigureServices((host, services) =>
                {
                    services.AddSingleton<WaitingForAppStartupService>();
                    services.AddSingleton<UdpSender>();
                    services.Configure<TestsSettings>(configuration.GetSection("TestsSettings"));
                });
            });

        await _workerApplicationFactory.StartAsync();
    }
    
    [Test]
    public async Task LbsServiceTest()
    {
        var settings = _workerApplicationFactory.Services.GetRequiredService<IOptions<TestsSettings>>().Value;
        var sender = _workerApplicationFactory.Services.GetRequiredService<UdpSender>();
        
        var cellsCoords = GetCoordinates(Coords.Cells);
        
        var tokenSource = new CancellationTokenSource();
        await sender.StartAsync(tokenSource.Token);

        var client = new UdpClient(settings.ListeningPort);

        var receivedCoordsCount = 0;

        while (receivedCoordsCount < 10)
        {
            var result = await client.ReceiveAsync(tokenSource.Token);

            if (Coordinates.TryParse(Encoding.UTF8.GetString(result.Buffer), out var coords))
            {
                Assert.AreEqual(cellsCoords[receivedCoordsCount], coords);

                receivedCoordsCount++;
            }
        }

        tokenSource.Cancel();
    }

    [TearDown]
    public async Task DoAfterEach()
    {
        await _workerApplicationFactory.StopAsync();
    }
}