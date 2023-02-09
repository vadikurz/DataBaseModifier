using System.Globalization;
using System.IO.Compression;
using System.Text;

namespace DataBaseModifier;

public class CsvHelper
{
    public async Task ReadAndWrite(GZipStream stream, string originalFileOutputPath, string modifiedFileOutputPath,
        char delimiter)
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

        using var streamReader = new StreamReader(stream);
        await using var modifiedStreamWriter = File.CreateText(modifiedFileOutputPath);
        await using var originalStreamWriter = File.CreateText(originalFileOutputPath);


        var resultLine = new StringBuilder();

        while (await streamReader.ReadLineAsync() is { } line)
        {
            await originalStreamWriter.WriteLineAsync(line);

            resultLine.Clear();

            var lowRange = line.IndexOf(delimiter);

            if (lowRange is -1)
                continue;

            if (lowRange != 3 || line[0] != 'G' || line[1] != 'S' || line[2] != 'M')
                continue;

            var upperRange = line.IndexOf(delimiter, lowRange + 1);
            
            if(upperRange is -1)
                continue;

            int.TryParse(line.AsSpan()[++lowRange..upperRange], out var MCC);

            resultLine.Append(MCC);
            resultLine.Append(delimiter);

            lowRange = upperRange;
            upperRange = line.IndexOf(delimiter, lowRange + 1);
            
            if(upperRange is -1)
                continue;

            int.TryParse(line.AsSpan()[++lowRange..upperRange], out var MNC);

            resultLine.Append(MNC);
            resultLine.Append(delimiter);

            lowRange = upperRange;
            upperRange = line.IndexOf(delimiter, lowRange + 1);
            
            if(upperRange is -1)
                continue;

            int.TryParse(line.AsSpan()[++lowRange..upperRange], out var LAC_TAC_NID);

            resultLine.Append(LAC_TAC_NID);
            resultLine.Append(delimiter);

            lowRange = upperRange;
            upperRange = line.IndexOf(delimiter, lowRange + 1);
            
            if(upperRange is -1)
                continue;

            int.TryParse(line.AsSpan()[++lowRange..upperRange], out var CID);

            resultLine.Append(CID);
            resultLine.Append(delimiter);

            lowRange = upperRange;
            upperRange = line.IndexOf(delimiter, lowRange + 1);
            
            if(upperRange is -1)
                continue;

            lowRange = upperRange;
            upperRange = line.IndexOf(delimiter, lowRange + 1);
            
            if(upperRange is -1)
                continue;

            double.TryParse(line.AsSpan()[++lowRange..upperRange], out var LongitudeEW);

            resultLine.Append(LongitudeEW);
            resultLine.Append(delimiter);

            lowRange = upperRange;
            upperRange = line.IndexOf(delimiter, lowRange + 1);
            
            if(upperRange is -1)
                continue;

            double.TryParse(line.AsSpan()[++lowRange..upperRange], out var LongitudeNS);

            resultLine.Append(LongitudeNS);

            await modifiedStreamWriter.WriteLineAsync(resultLine);
        }
    }
}