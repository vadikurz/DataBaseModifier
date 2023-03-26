using System.Globalization;
using System.Text;

namespace Common.Models;

public record Point(DateTime Time, Coordinates Coords, int Sat, Lbs Lbs)
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

        point = new Point(timeStamp, new Coordinates(lat, lng), sat, new Lbs(mcc, mnc, lac, cid));

        return true;
    }

    public override string ToString()
    {
        const char delimiter = ',';

        var builder = new StringBuilder();

        builder.Append(Time.ToString(CultureInfo.InvariantCulture));
        builder.Append(delimiter);
        builder.Append(Coords);
        builder.Append(delimiter);
        builder.Append(Sat);
        builder.Append(delimiter);
        builder.Append(Lbs);

        return builder.ToString();
    }
}