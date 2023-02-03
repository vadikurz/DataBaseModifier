using System.IO.Compression;

namespace Downloader;

public class OpenCelidService
{
    private HttpClient client;
    
    public string OutputDirectoryPath { get; set; }

    public string Token { get; set; } = "pk.f48f741b488ae7c9a645c3fb04171bef";

    public OpenCelidService(string path)
    {
        client = new HttpClient();
        OutputDirectoryPath = path;
    }

    public async Task DownloadAsync()
    {
        var url = $"https://opencellid.org/ocid/downloads?token={Token}&type=mcc&file=257.csv.gz";

        var stream = await client.GetStreamAsync(url);

        await Decompress(stream);
    }

    private async Task Decompress(Stream stream)
    {
        await using var targetStream = File.Create(Path.Combine(OutputDirectoryPath, "257downloaded.csv"));
        await using var decompressionStream = new GZipStream(stream, CompressionMode.Decompress);

        var bufferSize = 10_550_000;

        await decompressionStream.CopyToAsync(targetStream, bufferSize);
    }
}