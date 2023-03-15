using System.Globalization;

namespace TestDataGenerator.Models;

public class TowerParameters
{
    public int MCC { get; set; }
    
    public int MNC { get; set; }
    
    public int LAC_TAC_NID { get; set; }
    
    public int CID { get; set; }
    
    public double Latitude { get; set; }
    
    public double Longitude { get; set; }

    public TowerParameters(int mcc, int mnc, int lacTacNid, int cid, double latitude, double longitude)
    {
        MCC = mcc;
        MNC = mnc;
        LAC_TAC_NID = lacTacNid;
        CID = cid;
        Latitude = latitude;
        Longitude = longitude;
    }

    public static TowerParameters Parse(string towerParameters)
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        
        var delimiter = ",";

        var parameters = towerParameters.Split(delimiter);

        int.TryParse(parameters[0], out var mcc);
    
        int.TryParse(parameters[1], out var mnc);
    
        int.TryParse(parameters[2], out var lac_tac_nid);
    
        int.TryParse(parameters[3], out var cid);
    
        double.TryParse(parameters[4], out var longitude);
    
        double.TryParse(parameters[5], out var latitude);

        return new TowerParameters(mcc, mnc, lac_tac_nid, cid, latitude, longitude);
    }

    public override string ToString() => $"{MCC},{MNC},{LAC_TAC_NID},{CID},{Latitude},{Longitude}";
}