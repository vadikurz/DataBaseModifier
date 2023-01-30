using System.Globalization;
using DataBaseModifier.Models;

namespace DataBaseModifier;

public class CsvFileReader
{
    private char delimiter;
    private string path;
    private IFormatProvider formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };

    public CsvFileReader(string path, char delimiter)
    {
        this.path = path;
        this.delimiter = delimiter;
    }
    
    public IEnumerable<OriginalVievModel> Read()
    { 
        using var streamReader = File.OpenText(path);
        
        string line;

        while ((line = streamReader.ReadLine()) != null)
        {
            yield return CreateObjectFromString(line, delimiter);
        }
    }

    public OriginalVievModel CreateObjectFromString(string line, char delimiter)
    {
        ReadOnlySpan<char> buffer = line;

        var obj = new OriginalVievModel();

        var firstDelimiter = GetIndexOfDelimiter(buffer, 0, ',');

        obj.Radio = buffer[..firstDelimiter].ToString();

        int MCC;
        int MNC;
        int LAC_TAC_NID;
        int CID;
        int X;
        double LongitudeEW;
        double LongitudeNS;
        int Range;
        int Samples;
        int Changeable;
        long Created;
        long Updated;
        int AverageSignal;

        var indexOf2Delimiter = GetIndexOfDelimiter(buffer, firstDelimiter, delimiter);

        int.TryParse(buffer.Slice(firstDelimiter + 1, indexOf2Delimiter - firstDelimiter - 1), out MCC);
        obj.MCC = MCC;

        var indexOf3Delimiter = GetIndexOfDelimiter(buffer, indexOf2Delimiter, delimiter);

        int.TryParse(buffer.Slice(indexOf2Delimiter + 1, indexOf3Delimiter - indexOf2Delimiter - 1), out MNC);
        obj.MNC = MNC;

        var indexOf4Delimiter = GetIndexOfDelimiter(buffer, indexOf3Delimiter, delimiter);

        int.TryParse(buffer.Slice(indexOf3Delimiter + 1, indexOf4Delimiter - indexOf3Delimiter - 1), out LAC_TAC_NID);
        obj.LAC_TAC_NID = LAC_TAC_NID;

        var indexOf5Delimiter = GetIndexOfDelimiter(buffer, indexOf4Delimiter, delimiter);

        int.TryParse(buffer.Slice(indexOf4Delimiter + 1, indexOf5Delimiter - indexOf4Delimiter - 1), out CID);
        obj.CID = CID;

        var indexOf6Delimiter = GetIndexOfDelimiter(buffer, indexOf5Delimiter, delimiter);

        int.TryParse(buffer.Slice(indexOf5Delimiter + 1, indexOf6Delimiter - indexOf5Delimiter - 1), out X);
        obj.X = X;

        var indexOf7Delimiter = GetIndexOfDelimiter(buffer, indexOf6Delimiter, delimiter);

        double.TryParse(buffer.Slice(indexOf6Delimiter + 1, indexOf7Delimiter - indexOf6Delimiter - 1), NumberStyles.AllowDecimalPoint,
            formatter, out LongitudeEW);
        obj.LongitudeEW = LongitudeEW;

        var indexOf8Delimiter = GetIndexOfDelimiter(buffer, indexOf7Delimiter, delimiter);

        double.TryParse(buffer.Slice(indexOf7Delimiter + 1, indexOf8Delimiter - indexOf7Delimiter - 1), NumberStyles.AllowDecimalPoint,
            formatter, out LongitudeNS);
        obj.LongitudeNS = LongitudeNS;

        var indexOf9Delimiter = GetIndexOfDelimiter(buffer, indexOf8Delimiter, delimiter);

        int.TryParse(buffer.Slice(indexOf8Delimiter + 1, indexOf9Delimiter - indexOf8Delimiter - 1), out Range);
        obj.Range = Range;

        var indexOf10Delimiter = GetIndexOfDelimiter(buffer, indexOf9Delimiter, delimiter);

        int.TryParse(buffer.Slice(indexOf9Delimiter + 1, indexOf10Delimiter - indexOf9Delimiter - 1), out Samples);
        obj.Samples = Samples;

        var indexOf11Delimiter = GetIndexOfDelimiter(buffer, indexOf10Delimiter, delimiter);

        int.TryParse(buffer.Slice(indexOf10Delimiter + 1, indexOf11Delimiter - indexOf10Delimiter - 1), out Changeable);
        obj.Changeable = Changeable;

        var indexOf12Delimiter = GetIndexOfDelimiter(buffer, indexOf11Delimiter, delimiter);

        long.TryParse(buffer.Slice(indexOf11Delimiter + 1, indexOf12Delimiter - indexOf11Delimiter - 1), out Created);
        obj.Created = Created;

        var indexOf13Delimiter = GetIndexOfDelimiter(buffer, indexOf12Delimiter, delimiter);

        long.TryParse(buffer.Slice(indexOf12Delimiter + 1, indexOf13Delimiter - indexOf12Delimiter - 1), out Updated);
        obj.Updated = Updated;

        int.TryParse(buffer.Slice(indexOf13Delimiter + 1, line.Length - 1 - indexOf13Delimiter), out AverageSignal);
        obj.AverageSignal = AverageSignal;

        return obj;
    }

    private static int GetIndexOfDelimiter(ReadOnlySpan<char> buffer, int indexOfPreviousDelimiter, char delimiter)
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