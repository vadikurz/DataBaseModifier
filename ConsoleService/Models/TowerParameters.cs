using System.Globalization;

namespace ConsoleService.Models;

public record TowerParameters(int Mcc, int Mnc, int Lac, int Cid, double Lat, double Lng)
{
    public static bool TryParse(string line, out TowerParameters towerParameters)
    {
        const char delimiter = ',';
        
        var style = NumberStyles.Number;

        var parameters = line.Split(delimiter);

        towerParameters = default;

        if (!int.TryParse(parameters[0], out var mcc))
            return false;

        if (!int.TryParse(parameters[1], out var mnc))
            return false;

        if (!int.TryParse(parameters[2], out var lacTacNid))
            return false;

        if (!int.TryParse(parameters[3], out var cid))
            return false;

        if (!double.TryParse(parameters[4], style, CultureInfo.InvariantCulture, out var longitude))
            return false;

        if (!double.TryParse(parameters[5], style, CultureInfo.InvariantCulture, out var latitude))
            return false;

        towerParameters = new TowerParameters(mcc, mnc, lacTacNid, cid, latitude, longitude);

        return true;
    }
}