using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace DataBaseModifier;

public class CsvFileWriter<T>
{
    public static void Write(string path, string delimiter, IEnumerable<T> data)
    {
        using var streamWriter = new StreamWriter(path);
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            { Delimiter = delimiter, HasHeaderRecord = false };

        using var csvWriter = new CsvWriter(streamWriter, csvConfig);
        csvWriter.WriteRecords(data);
    }
}