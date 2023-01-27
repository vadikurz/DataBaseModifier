using System.Globalization;

namespace DataBaseModifier;

public class CsvFileReader<T> where T : new()
{
    private static readonly IFormatProvider Formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };

    public static IEnumerable<T> Read(string path, string delimiter)
    {
        using var streamReader = File.OpenText(path);
        string line;

        while ((line = streamReader.ReadLine()) != null)
        {
            yield return ConstructObjectFromLine(line, delimiter);
        }
    }

    private static T ConstructObjectFromLine(string line, string delimiter)
    {
        var array = line.Split(delimiter);
        var record = new T();
        var propertiesOfInstance = record.GetType().GetProperties();

        for (var i = 0; i < propertiesOfInstance.Length; i++)
        {
            var value = Convert.ChangeType(array[i], propertiesOfInstance[i].PropertyType, Formatter);

            propertiesOfInstance[i].SetValue(record, value);
        }

        return record;
    }
}