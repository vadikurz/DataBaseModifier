using Downloader;

namespace DataBaseModifier;

public class Program
{
    private const string OutputPrefix = "modified";

    /// <param name="args">args[0] is the path to file, args[1] is the file name</param>
    public static async Task Main(string[] args)
    {
        string path, fileName;

        if (args.Length == 0)
        {
            path = Environment.CurrentDirectory;
            fileName = "257.csv";
        }
        else if (args.Length == 1)
        {
            path = args[0];
            fileName = "257.csv";
        }
        else
        {
            path = args[0];
            fileName = args[1];
        }


        if (!Directory.Exists(path))
        {
            Console.WriteLine("Usage: Downloader.exe [<output dir>]");
            Environment.Exit(-1);
        }

        if (!fileName.EndsWith(".csv"))
        {
            Console.WriteLine("Usage: Downloader.exe [<filename>]");
            Environment.Exit(-1);
        }

        using var service = new OpenCellidService();
        await service.DownloadAndDecompressAsync("257.csv.gz", path);

        var csvHelper = new CsvHelper();
        csvHelper.ReadAndWrite(Path.Combine(path, fileName), Path.Combine(path, OutputPrefix + fileName), ',');
    }
}