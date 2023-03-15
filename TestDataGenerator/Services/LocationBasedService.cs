using TestDataGenerator.Models;

namespace TestDataGenerator.Services;

public class LocationBasedService
{
    public async Task<List<TowerParameters>> GetListOfTowerParameters(string pathToTowerParameters)
    {
        var listOfTowerParameters = new List<TowerParameters>();

        var streamReader = File.OpenText(pathToTowerParameters);

        while (await streamReader.ReadLineAsync() is { } line)
        {
            var parameters = TowerParameters.Parse(line);

            listOfTowerParameters.Add(parameters);
        }

        return listOfTowerParameters;
    }

    public async Task<List<Point>> GetListOfObjectCoordinates(string pathToObjectCoordinates)
    {
        var listOfObjectCoordinates = new List<Point>();

        var streamReader = File.OpenText(pathToObjectCoordinates);

        while (await streamReader.ReadLineAsync() is { } line)
        {
            listOfObjectCoordinates.Add(Point.Parse(line));
        }

        return listOfObjectCoordinates;
    }

    public List<LBSParameters> GetLbsParametersOfNearestTowers(List<Point> listOfObjectCoordinates,
        List<TowerParameters> listOfTowerParameters) => 
        listOfObjectCoordinates
            .Select(objectCoordinates => listOfTowerParameters
                .MinBy(towerParameters => CalculateDistance(towerParameters.Latitude, objectCoordinates.Latitude,
                    towerParameters.Longitude, objectCoordinates.Longitude)))
            .Select(towerParameters =>
                new LBSParameters(towerParameters.MCC, towerParameters.MNC, towerParameters.LAC_TAC_NID, towerParameters.CID))
            .ToList();
    

    private double CalculateDistance(double x1, double x2, double y1, double y2) =>
        Math.Sqrt(Math.Pow(x1 - x2, 2) +
                  Math.Pow(y1 - y2, 2));
}