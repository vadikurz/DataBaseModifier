namespace ConsoleService;

public interface IUdpReceiver
{
    public Task ReceiveAsync();
}