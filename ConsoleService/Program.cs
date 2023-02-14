using ConsoleService;

public class Program
{
    public static async Task Main(string[] args)
    {
        var service = new ConsoleBackgroundService();

        var cancellationTokenSource = new CancellationTokenSource();
        var token = cancellationTokenSource.Token;

        await service.StartAsync(token);

        while (Console.ReadKey().Key != ConsoleKey.X)
        {
            
        }
        
        cancellationTokenSource.Cancel();
    }
}