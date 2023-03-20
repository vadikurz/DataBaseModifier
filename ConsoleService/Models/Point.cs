using System.Globalization;
using System.Text;

namespace ConsoleService.Models;

public record Point(DateTime Time, double Lat, double Lng, int Sat, int Mcc, int Mnc, int Lac,
    int Cid)
{
    public static bool TryParse(string line, out Point? point)
    {
        point = default;

        var parameters = line.Split(',');

        if (!DateTime.TryParse(parameters[0], CultureInfo.InvariantCulture, DateTimeStyles.None, out var timeStamp)||
            !double.TryParse(parameters[1], NumberStyles.Number, CultureInfo.InvariantCulture, out var lat)||
            !double.TryParse(parameters[2], NumberStyles.Number, CultureInfo.InvariantCulture, out var lng)||
            !int.TryParse(parameters[3], out var sat)||
            !int.TryParse(parameters[4], out var mcc)||
            !int.TryParse(parameters[5], out var mnc)||
            !int.TryParse(parameters[6], out var lac)||
            !int.TryParse(parameters[7], out var cid))
            return false;

        point = new Point(timeStamp, lat, lng, sat, mcc, mnc, lac, cid);

        return true;
    }

    public override string ToString()
    {
        const char delimiter = ',';

        var builder = new StringBuilder();

        builder.Append(Time.ToString(CultureInfo.InvariantCulture));
        builder.Append(delimiter);
        builder.Append(Lat.ToString(CultureInfo.InvariantCulture));
        builder.Append(delimiter);
        builder.Append(Lng.ToString(CultureInfo.InvariantCulture));
        builder.Append(delimiter);
        builder.Append(Sat);
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