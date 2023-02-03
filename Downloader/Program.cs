namespace Downloader;

public class Program
{
    /// <param name="args">args[0] is the output directory for file</param>
    public static async Task Main(string[] args)
    {
        var service = new OpenCelidService(args[0]);
        await service.DownloadAsync();
    }
}