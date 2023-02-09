using System.IO.Compression;

namespace DataBaseModifier;

public class Program
{
    private const string OutputPrefix = "modified";

    /// <param name="args">args[0] is the path to file, args[1] is the file name, args[2] is the token</param>
    public static async Task Main(string[] args)
    {
        var path = Environment.CurrentDirectory;
        var fileName = "257.csv.gz";
        var token = "pk.099507f0795cf7dbfb07161f2ad6a4b6";

        if (args.Length > 0)
            path = args[0];

        if (args.Length > 1)
            fileName = args[1];

        if (args.Length > 2)
            token = args[2];

        if (!Directory.Exists(path))
        {
            Console.WriteLine("Usage: Downloader.exe [<output dir>]");
            Environment.Exit(-1);
        }

        if (!fileName.EndsWith(".gz"))
        {
            throw new ArgumentException("Expected (*.gz)", nameof(fileName));
        }

        using var client = new HttpClient();

        var url = $"https://opencellid.org/ocid/downloads?token={token}&type=mcc&file={fileName}";

        await using var stream = await client.GetStreamAsync(url);
        await using var decompressionStream = new GZipStream(stream, CompressionMode.Decompress);

        var csvHelper = new CsvHelper();

        await csvHelper.ReadAndWrite(decompressionStream, Path.Combine(path, fileName[..^".gz".Length]),
           Path.Combine(path, OutputPrefix + fileName[..^".gz".Length]), ',');
           await csvHelper.ReadAndWrite(decompressionStream, path+"257.csv",
          path + "modified257.csv", ',');
    }
}