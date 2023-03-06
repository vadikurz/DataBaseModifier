using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Options;

namespace ConsoleService;

public class UdpSender : IUdpSender, IDisposable
{
    private readonly UdpClient client;

    public UdpSender(IOptions<UdpSettings> settings)
    {
        client = new UdpClient(settings.Value.Ip, settings.Value.Port);
    }
    
    public async Task SendAsync()
    {
        var message = DateTimeOffset.Now.ToString();
        var data = Encoding.UTF8.GetBytes(message);

        await client.SendAsync(data);
    }

    public void Dispose() => client.Dispose();
}