namespace DataBaseModifier.Models;

public class OriginalVievModel
{
    public string Radio { get; set; }
    public int MCC { get; set; }
    public int MNC { get; set; }
    public int LAC_TAC_NID { get; set; }
    public int CID { get; set; }
    public int X { get; set; }
    public double LongitudeEW { get; set; }
    public double LongitudeNS { get; set; }
    public int Range { get; set; }
    public int Samples { get; set; }
    public int Changeable { get; set; }
    public long Created { get; set; }
    public long Updated { get; set; }
    public int AverageSignal { get; set; }
}