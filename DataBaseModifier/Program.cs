using DataBaseModifier.Models;

namespace DataBaseModifier;

public class Program
{
    private const string OutputPrefix = "modified";

    /// <param name="args">args[0] is the path to file, args[1] is the file name</param>
    public static void Main(string[] args)
    {
        var reader = new CsvFileReader(Path.Combine(args), ',');
        var writer = new CsvFileWriter(Path.Combine(args[0], OutputPrefix + args[1]), ',');

        var records = reader.Read();

        var gsmRecords = records
            .Where(record => record.Radio == "GSM")
            .Select(
                record =>
                    new SubViewModel
                    {
                        MCC = record.MCC,
                        MNC = record.MNC,
                        LAC_TAC_NID = record.LAC_TAC_NID,
                        CID = record.CID,
                        LongitudeEW = record.LongitudeEW,
                        LongitudeNS = record.LongitudeNS
                    });

        writer.Write(gsmRecords);
    }
}