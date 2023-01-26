using DataBaseModifier.Models;

namespace DataBaseModifier;

public class Program
{
    private const string OutputPrefix = "modified";
    
    /// <param name="args">args[0] is the path to file, args[1] is the file name</param>
    public static void Main(string[] args)
    {
        var records = CsvFileReader<OriginalVievModel>.Read(Path.Combine(args), ",");
        
        CsvFileWriter<SubViewModel>.Write(Path.Combine(args[0], OutputPrefix + args[1]), ",", records
            .Select(
                record =>
                    new SubViewModel
                    {
                        Radio = record.Radio,
                        MCC = record.MCC,
                        MNC = record.MNC,
                        LAC_TAC_NID = record.LAC_TAC_NID,
                        CID = record.CID,
                        LongitudeEW = record.LongitudeEW,
                        LongitudeNS = record.LongitudeNS
                    }));
    }
}