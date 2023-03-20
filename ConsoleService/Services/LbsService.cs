using ConsoleService.Models;

namespace ConsoleService.Services;

public class LbsService
{
    private readonly Dictionary<LbsParameters, TowerParameters> _towers = GetTowers();

    public Coordinates GetCoordinates(Point point)
    {
        if (point.Sat < 3)
        {
            if (_towers.TryGetValue(
                    new LbsParameters(point.Mcc, point.Mnc, point.Lac, point.Cid),
                    out var tower))
                return new Coordinates(tower.Lat, tower.Lng);
        }

        return new Coordinates(point.Lat, point.Lng);
    }

    private static Dictionary<LbsParameters, TowerParameters> GetTowers()
    {
        var towersArray = TestData.Towers.Split("\r\n");

        var towers = new Dictionary<LbsParameters, TowerParameters>(towersArray.Length);

        foreach (var tower in towersArray)
        {
            if (TowerParameters.TryParse(tower, out var towerParams))
                towers.Add(new LbsParameters(towerParams.Mcc, towerParams.Mnc, towerParams.Lac,
                    towerParams.Cid), towerParams);
        }

        return towers;
    }

    public LbsParameters FindLbs(Point point)
    {
        var (_, value) = _towers.MinBy(tower =>
            CalculateDistance(tower.Value.Lat, point.Lat, tower.Value.Lng, point.Lng));

        return new LbsParameters(value.Mcc, value.Mnc, value.Lac, value.Cid);
    }

    private double CalculateDistance(double x1, double x2, double y1, double y2) =>
        Math.Pow(x1 - x2, 2) +
        Math.Pow(y1 - y2, 2);
}