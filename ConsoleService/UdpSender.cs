using System.Net.Sockets;
using System.Text;

namespace ConsoleService;

public class UdpSender : IUdpSender, IDisposable
{
    private readonly UdpClient client;

    public UdpSender(string ipAddress, int port)
    {
        client = new UdpClient(ipAddress, port);
    }
    
    public async Task SendAsync()
    {
        var message = DateTimeOffset.Now.ToString();
        var data = Encoding.UTF8.GetBytes(message);

        await client.SendAsync(data);
    }

    public void Dispose() => client.Dispose();
}