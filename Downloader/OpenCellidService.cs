using System.IO.Compression;

namespace Downloader;

public sealed class OpenCellidService : IDisposable
{
    private readonly HttpClient client = new();

    public string Token { get; set; } = "pk.f48f741b488ae7c9a645c3fb04171bef";

    public async Task DownloadAndDecompressAsync(string fileName, string path)
    {
        if (!fileName.EndsWith(".gz"))
        {
            throw new ArgumentException("Expected (*.gz)", nameof(fileName));
        }
        
        var url = $"https://opencellid.org/ocid/downloads?token={Token}&type=mcc&file={fileName}";

        await using var stream = await client.GetStreamAsync(url);
        await using var decompressionStream = new GZipStream(stream, CompressionMode.Decompress);
        await using var targetStream = File.Create(Path.Combine(path ,fileName[..^".gz".Length]));
        await decompressionStream.CopyToAsync(targetStream);
    }

    public void Dispose() => client.Dispose();
}