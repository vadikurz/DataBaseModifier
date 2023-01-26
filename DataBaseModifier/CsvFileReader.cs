using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;

namespace DataBaseModifier;

public class CsvFileReader<T>
{
    public static IEnumerable<T> Read(string path, string delimiter)
    {
        using var streamReader = new StreamReader(path);
        var csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            { Delimiter = delimiter, HasHeaderRecord = false };

        using var csvReader = new CsvReader(streamReader, csvConfig);
        foreach (var record in csvReader.GetRecords<T>())
        {
            yield return record;
        }
    }
}