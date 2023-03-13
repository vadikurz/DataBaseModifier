using System.Net.Sockets;
using System.Text;
using ConsoleService.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace ConsoleService.Services;

public class UdpSender : ApplicationBackgroundService
{
    private readonly UdpClient _client;
    private readonly StreamReader _streamReader;

    public UdpSender(IHostApplicationLifetime lifetime, IOptions<UdpSettings> udpSettings, IOptions<DataSourceSettings> dataSourceSettings) : base(lifetime)
    {
        _client = new UdpClient(udpSettings.Value.Ip, udpSettings.Value.Port);
        _streamReader = File.OpenText(dataSourceSettings.Value.FilePath);
    }

    protected override async Task ExecuteOnApplicationStartAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            while (await _streamReader.ReadLineAsync() is { } line)
            {
                var data = Encoding.UTF8.GetBytes(line);
                
                await _client.SendAsync(data, stoppingToken);
                
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}