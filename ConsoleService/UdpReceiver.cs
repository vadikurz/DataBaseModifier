﻿using System.Net.Sockets;
using System.Text;
using Microsoft.Extensions.Options;

namespace ConsoleService;

public class UdpReceiver : IUdpReceiver, IDisposable
{
    private readonly UdpClient client;

    public UdpReceiver(IOptions<UdpSettings> settings)
    {
        client = new UdpClient(settings.Value.Port);
    }

    public async Task ReceiveAsync()
    {
        var result = await client.ReceiveAsync();
        var message = Encoding.UTF8.GetString(result.Buffer);
        
        Console.WriteLine(message);
    }

    public void Dispose() => client.Dispose();
}