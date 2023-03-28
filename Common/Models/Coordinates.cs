using System.Globalization;

namespace Common.Models;

public record Coordinates(double Lat, double Lng)
{
    public override string ToString()
    {
        var latString = Lat.ToString(CultureInfo.InvariantCulture);
        var lngString = Lng.ToString(CultureInfo.InvariantCulture);

        return $"{latString},{lngString}";
    }

    public static bool TryParse(string line, out Coordinates? coords)
    {
        coords = default;

        var parameters = line.Split(',');

        if (!double.TryParse(parameters[0], NumberStyles.Number, CultureInfo.InvariantCulture, out var lat)||
            !double.TryParse(parameters[1], NumberStyles.Number, CultureInfo.InvariantCulture, out var lng))
            return false;

        coords = new Coordinates(lat, lng);

        return true;
    }
}
