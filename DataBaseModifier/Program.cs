using DataBaseModifier.Models;

namespace DataBaseModifier;

public class Program
{
    private const string OutputPrefix = "modified";

    public static void Main(string[] args)
    {
        var records = CsvFileReader<OriginalVievModel>.Read(Path.Combine(args[0], args[1]), ",");
        CsvFileWriter<SubViewModel>.Write(Path.Combine(args[0], OutputPrefix + args[1]), ",", records
            .Select(
                obj =>
                    new SubViewModel
                    {
                        Radio = obj.Radio,
                        MCC = obj.MCC,
                        MNC = obj.MNC,
                        LAC_TAC_NID = obj.LAC_TAC_NID,
                        CID = obj.CID,
                        LongitudeEW = obj.LongitudeEW,
                        LongitudeNS = obj.LongitudeNS
                    }));
    }
}