using System.Globalization;
using DataBaseModifier.Models;

namespace DataBaseModifier;

public class CsvFileWriter
{
    private char delimiter;
    private string path;
    private IFormatProvider Formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
    
    public CsvFileWriter(string path, char delimiter)
    {
        this.path = path;
        this.delimiter = delimiter;
    }
    
    public void Write(IEnumerable<SubViewModel> data)
    {
        using var fileStream = File.OpenWrite(path);
        using var streamWriter = new StreamWriter(fileStream);

        foreach (var obj in data)
        {
            streamWriter.Write(obj.MCC);
            streamWriter.Write(delimiter);
            streamWriter.Write(obj.MNC);
            streamWriter.Write(delimiter);
            streamWriter.Write(obj.LAC_TAC_NID);
            streamWriter.Write(delimiter);
            streamWriter.Write(obj.CID);
            streamWriter.Write(delimiter);
            streamWriter.Write(Convert.ToString(obj.LongitudeEW, Formatter));
            streamWriter.Write(delimiter);
            streamWriter.Write(Convert.ToString(obj.LongitudeNS, Formatter));
            streamWriter.WriteLine();
        }
    }
}