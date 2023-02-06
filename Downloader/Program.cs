namespace Downloader;

public class Program
{
    /// <param name="args">args[0] is the output directory for file</param>
    public static async Task Main(string[] args)
    {
        var path = args.Any() ? args[0] : Environment.CurrentDirectory;

        if (!Directory.Exists(path))
        {
            Console.WriteLine("Usage: Downloader.exe [<output dir>]");
            Environment.Exit(-1);
        }
        
        using var service = new OpenCellidService();
        await service.DownloadAndDecompressAsync("257.csv.gz", path);
    }
}