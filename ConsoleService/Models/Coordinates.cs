using System.Globalization;

namespace ConsoleService.Models;

public record Coordinates(double Lat, double Lng)
{
    public override string ToString()
    {
        var latString = Lat.ToString(CultureInfo.InvariantCulture);
        var lngString = Lng.ToString(CultureInfo.InvariantCulture);

        return $"{latString},{lngString}";
    }
}
