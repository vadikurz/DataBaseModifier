using ConsoleService.Models;

namespace ConsoleService.Services;

public class LbsService
{
    private readonly Lazy<Dictionary<Lbs, TowerParameters>> _lazy = new(() =>
    {
        var cells = new Dictionary<Lbs, TowerParameters>();

        using var streamReader = File.OpenText("cells.csv");

        while (streamReader.ReadLine() is { } line)
        {
            if (TowerParameters.TryParse(line, out var cell))
                cells.Add(new Lbs(cell.Mcc, cell.Mnc, cell.Lac, cell.Cid), cell);
        }

        return cells;
    });

    private Dictionary<Lbs, TowerParameters> Cells => _lazy.Value;

    public bool TryGetCoordinates(Point point, out Coordinates? coords)
    {
        coords = default;

        if (!Cells.TryGetValue(new Lbs(point.Lbs.Mcc, point.Lbs.Mnc, point.Lbs.Lac, point.Lbs.Cid), out var cell))
            return false;

        coords = new Coordinates(cell.Lat, cell.Lng);
        
        return true;
    }
}