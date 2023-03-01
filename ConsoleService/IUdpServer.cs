namespace ConsoleService;

public interface IUdpServer
{
    public Task ReceiveAsync();

    public Task SendAsync();
}