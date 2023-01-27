using System.Globalization;

namespace DataBaseModifier;

public class CsvFileWriter<T>
{
    private static readonly IFormatProvider Formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };

    public static void Write(string path, string delimiter, IEnumerable<T> data)
    {
        using var fileStream = File.OpenWrite(path);
        using var streamWriter = new StreamWriter(fileStream);

        foreach (var obj in data)
        {
            streamWriter.WriteLine(TransformToLine(obj, delimiter));
        }
    }

    private static string TransformToLine(T obj, string delimiter)
    {
        var propertyValuesStrings = typeof(T).GetProperties()
            .Select(property =>
                Convert.ChangeType(property.GetValue(obj), typeof(string), Formatter));

        return string.Join(delimiter, propertyValuesStrings);
    }
}