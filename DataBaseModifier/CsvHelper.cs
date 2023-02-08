using System.Globalization;
using System.IO.Compression;
using System.Text;

namespace DataBaseModifier;

public class CsvHelper
{
    public void ReadAndWrite(GZipStream stream,string originalFileOutputPath,string modifiedFileOutputPath,  char delimiter)
    { 
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        
        using var streamReader = new StreamReader(stream);
        using var modifiedFileStream = File.OpenWrite(modifiedFileOutputPath);
        using var originalFileStream = File.OpenWrite(originalFileOutputPath);
        using var modifiedStreamWriter = new StreamWriter(modifiedFileStream);
        using var originalStreamWriter = new StreamWriter(originalFileStream);
        
        string line;
        var resultLine = new StringBuilder();
        
        int MCC;
        int MNC;
        int LAC_TAC_NID;
        int CID;
        double LongitudeEW;
        double LongitudeNS;

        while ((line = streamReader.ReadLine()) != null)
        {
            originalStreamWriter.WriteLine(line);
            
            resultLine.Clear();
            
            var firstDelimiter = line.IndexOf(delimiter);

            if (line[..firstDelimiter] != "GSM")
            {
                continue;
            }

            ReadOnlySpan<char> buffer = line.AsSpan();
            
            var indexOf2Delimiter = GetIndexOfDelimiter(buffer, firstDelimiter, delimiter);
            
            int.TryParse(buffer.Slice(firstDelimiter + 1, indexOf2Delimiter - firstDelimiter - 1), out MCC);
            
            resultLine.Append(MCC);
            resultLine.Append(delimiter);
            
            var indexOf3Delimiter = GetIndexOfDelimiter(buffer, indexOf2Delimiter, delimiter);

            int.TryParse(buffer.Slice(indexOf2Delimiter + 1, indexOf3Delimiter - indexOf2Delimiter - 1), out MNC);
            
            resultLine.Append(MNC);
            resultLine.Append(delimiter);
            
            var indexOf4Delimiter = GetIndexOfDelimiter(buffer, indexOf3Delimiter, delimiter);

            int.TryParse(buffer.Slice(indexOf3Delimiter + 1, indexOf4Delimiter - indexOf3Delimiter - 1), out LAC_TAC_NID);

            resultLine.Append(LAC_TAC_NID);
            resultLine.Append(delimiter);

            var indexOf5Delimiter = GetIndexOfDelimiter(buffer, indexOf4Delimiter, delimiter);

            int.TryParse(buffer.Slice(indexOf4Delimiter + 1, indexOf5Delimiter - indexOf4Delimiter - 1), out CID);
            
            resultLine.Append(CID);
            resultLine.Append(delimiter);
            
            var indexOf6Delimiter = GetIndexOfDelimiter(buffer, indexOf5Delimiter, delimiter);
            
            var indexOf7Delimiter = GetIndexOfDelimiter(buffer, indexOf6Delimiter, delimiter);

            double.TryParse(buffer.Slice(indexOf6Delimiter + 1, indexOf7Delimiter - indexOf6Delimiter - 1), out LongitudeEW);
            
            resultLine.Append(LongitudeEW);
            resultLine.Append(delimiter);

            var indexOf8Delimiter = GetIndexOfDelimiter(buffer, indexOf7Delimiter, delimiter);

            double.TryParse(buffer.Slice(indexOf7Delimiter + 1, indexOf8Delimiter - indexOf7Delimiter - 1), out LongitudeNS);
            
            resultLine.Append(LongitudeNS);

            modifiedStreamWriter.WriteLine(resultLine);
        }
    }

    private int GetIndexOfDelimiter(ReadOnlySpan<char> buffer, int indexOfPreviousDelimiter, char delimiter)
    {
        var index = 0;

        for (int i = indexOfPreviousDelimiter + 1; i < buffer.Length; i++)
        {
            if (buffer[i] == delimiter)
            {
                index = i;
                break;
            }
        }

        return index;
    }
}