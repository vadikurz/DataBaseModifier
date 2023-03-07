namespace ConsoleService;

public interface IUdpSender
{
    public Task SendAsync(string message);
}