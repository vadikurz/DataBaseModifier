using System.Globalization;
using System.Text;

namespace ConsoleService.Models;

public record Point(DateTime TimeStamp, double Latitude, double Longitude, int NumberOfSatellites, int Mcc, int Mnc, int LacTacNid, int Cid)
{
    public static bool TryParse(string line, out Point point)
    {
        var delimiter = ',';
        
        var formatter = new NumberFormatInfo { NumberDecimalSeparator = "." };
        var style = NumberStyles.Number;

        point = default;
        
        var parameters = line.Split(delimiter);

        if (!DateTime.TryParse(parameters[0], CultureInfo.InvariantCulture, DateTimeStyles.None,  out var timeStamp))
            return false;
        
        if (!double.TryParse(parameters[1],style, formatter , out var latitude))
            return false;

        if (!double.TryParse(parameters[2], style, formatter , out var longitude))
            return false;
        
        if (!int.TryParse(parameters[3], out var numberOfSatellites))
            return false;

        if (!int.TryParse(parameters[4], out var mcc))
            return false;

        if (!int.TryParse(parameters[5], out var mnc))
            return false;

        if (!int.TryParse(parameters[6], out var lacTacNid))
            return false;

        if (! int.TryParse(parameters[7], out var cid))
            return false;
        
        point = new Point(timeStamp, latitude, longitude, numberOfSatellites, mcc, mnc, lacTacNid, cid);

        return true;
    }

    public override string ToString()
    {
        var timeStamp = TimeStamp.ToString(CultureInfo.InvariantCulture);
        var latitude = Latitude.ToString(CultureInfo.InvariantCulture);
        var longitude = Longitude.ToString(CultureInfo.InvariantCulture);

        return $"{timeStamp},{latitude},{longitude},{NumberOfSatellites},{Mcc},{Mnc},{LacTacNid},{Cid}";
    }
    
    public byte[] Serialize() => Encoding.UTF8.GetBytes(ToString());
}