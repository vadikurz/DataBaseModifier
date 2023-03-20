using System.Globalization;
using System.Text;

namespace ConsoleService.Models;

public record Point(DateTime TimeStamp, double Lat, double Lng, int NumberOfSatellites, int Mcc, int Mnc, int Lac, int Cid)
{
    public static bool TryParse(string line, out Point? point)
    {
        const char delimiter = ',';
        
        var style = NumberStyles.Number;

        point = default;
        
        var parameters = line.Split(delimiter);

        if (!DateTime.TryParse(parameters[0], CultureInfo.InvariantCulture, DateTimeStyles.None,  out var timeStamp))
            return false;
        
        if (!double.TryParse(parameters[1],style, CultureInfo.InvariantCulture , out var latitude))
            return false;

        if (!double.TryParse(parameters[2], style, CultureInfo.InvariantCulture , out var longitude))
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
        const char delimiter = ',';
        
        var builder = new StringBuilder();

        builder.Append(TimeStamp.ToString(CultureInfo.InvariantCulture));
        builder.Append(delimiter);
        builder.Append(Lat.ToString(CultureInfo.InvariantCulture));
        builder.Append(delimiter);
        builder.Append(Lng.ToString(CultureInfo.InvariantCulture));
        builder.Append(delimiter);
        builder.Append(NumberOfSatellites);
        builder.Append(delimiter);
        builder.Append(Mcc);
        builder.Append(delimiter);
        builder.Append(Mnc);
        builder.Append(delimiter);
        builder.Append(Lac);
        builder.Append(delimiter);
        builder.Append(Cid);


        return builder.ToString();
    }
    
    public byte[] Serialize() => Encoding.UTF8.GetBytes(ToString());
}