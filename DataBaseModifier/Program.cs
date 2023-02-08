using System.IO.Compression;

namespace DataBaseModifier;

public class Program
{
    private const string OutputPrefix = "modified";

    /// <param name="args">args[0] is the path to file, args[1] is the file name</param>
    public static async Task Main(string[] args)
    {
        string path, fileName, token;

        var defaultPath = Environment.CurrentDirectory;
        var defaultFileName = "257.csv.gz";
        var defaultToken = "pk.f48f741b488ae7c9a645c3fb04171bef";

        if (args.Length == 0)
        {
            path = defaultPath;
            fileName = defaultFileName;
            token = defaultToken;
        }
        else if (args.Length == 1)
        {
            path = args[0];
            fileName = defaultFileName;
            token = defaultToken;
        }
        else if (args.Length == 2)
        {
            path = args[0];
            fileName = args[1];
            token = defaultToken;
        }
        else
        {
            path = args[0];
            fileName = args[1];
            token = args[2];
        }

        if (!Directory.Exists(path))
        {
            Console.WriteLine("Usage: Downloader.exe [<output dir>]");
            Environment.Exit(-1);
        }

        if (!fileName.EndsWith(".gz"))
        {
            throw new ArgumentException("Expected (*.gz)", nameof(fileName));
        }

        var client = new HttpClient();

        var url = $"https://opencellid.org/ocid/downloads?token={token}&type=mcc&file={fileName}";

        await using var stream = await client.GetStreamAsync(url);
        await using var decompressionStream = new GZipStream(stream, CompressionMode.Decompress);

        var csvHelper = new CsvHelper();

        csvHelper.ReadAndWrite(decompressionStream, Path.Combine(path, fileName[..^".gz".Length]),
            Path.Combine(path, OutputPrefix + fileName[..^".gz".Length]), ',');
    }
}