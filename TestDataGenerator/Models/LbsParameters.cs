namespace TestDataGenerator.Models;

public class LbsParameters
{
    public int MCC { get; set; }
    
    public int MNC { get; set; }
    
    public int LAC_TAC_NID { get; set; }
    
    public int CID { get; set; }

    public LbsParameters(int mcc, int mnc, int lacTacNid, int cid)
    {
        MCC = mcc;
        MNC = mnc;
        LAC_TAC_NID = lacTacNid;
        CID = cid;
    }

    public override string ToString() => $"{MCC},{MNC},{LAC_TAC_NID},{CID}";
}