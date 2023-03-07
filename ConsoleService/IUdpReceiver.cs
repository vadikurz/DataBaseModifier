namespace ConsoleService;

public interface IUdpReceiver
{
    public Task<string> ReceiveAsync();
}