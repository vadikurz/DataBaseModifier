using System.Net.Sockets;
using System.Text;

namespace ConsoleService;

public class UdpReceiver : IUdpReceiver, IDisposable
{
    private readonly UdpClient client;

    public UdpReceiver(int port)
    {
        client = new UdpClient(port);
    }

    public async Task ReceiveAsync()
    {
        var result = await client.ReceiveAsync();
        var message = Encoding.UTF8.GetString(result.Buffer);
        
        Console.WriteLine(message);
    }

    public void Dispose() => client.Dispose();
}