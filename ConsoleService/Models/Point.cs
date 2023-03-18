using System.Globalization;
using System.Text;

namespace ConsoleService.Models;

public class Point
{
    public DateTime TimeStamp { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public int NumberOfSatellites { get; set; }

    public int MCC { get; set; }

    public int MNC { get; set; }
    
    public int LAC_TAC_NID { get; set; }
    
    public int CID { get; set; }

    public Point(DateTime timeStamp, double latitude, double longitude, int numberOfSatellites, int mcc, int mnc, int lacTacNid, int cid)
    {
        TimeStamp = timeStamp;
        Latitude = latitude;
        Longitude = longitude;
        NumberOfSatellites = numberOfSatellites;
        MCC = mcc;
        MNC = mnc;
        LAC_TAC_NID = lacTacNid;
        CID = cid;
    }

    public static Point Parse(string line)
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        
        var delimiter = ',';
        
        var parameters = line.Split(delimiter);
        
        DateTime.TryParse(parameters[0],  out var timeStamp);

        double.TryParse(parameters[1], out var latitude);

        double.TryParse(parameters[2], out var longitude);

        int.TryParse(parameters[3], out var numberOfSatellites);
        
        int.TryParse(parameters[4], out var mcc);
    
        int.TryParse(parameters[5], out var mnc);
    
        int.TryParse(parameters[6], out var lac_tac_nid);
    
        int.TryParse(parameters[7], out var cid);

        return new Point(timeStamp, latitude, longitude, numberOfSatellites, mcc, mnc, lac_tac_nid, cid);
    }

    public override string ToString() =>
        $"{TimeStamp},{Latitude},{Longitude},{NumberOfSatellites},{MCC},{MNC},{LAC_TAC_NID},{CID}";

    public byte[] Serialize() => Encoding.UTF8.GetBytes(ToString());
}