using TestDataGenerator.Services;

namespace TestDataGenerator;

public class Program
{
    public static async Task Main(string[] args)
    {
        var locationBasedService = new LocationBasedService();

        var csvHelper = new CsvWriter();
        
        var towerParametersPath = args[0];
        var objectCoordinatesPath = args[1];
        var outputPath = args[2];
        var outputFileName = args[3];

        var listOfTowerParameters = await locationBasedService.GetListOfTowerParameters(towerParametersPath);

        var listOfObjectCoordinates = await locationBasedService.GetListOfObjectCoordinates(objectCoordinatesPath);

        var listOfLbsParameters =
            locationBasedService.GetLbsParametersOfNearestTowers(listOfObjectCoordinates, listOfTowerParameters);

        await csvHelper.Write(Path.Combine(outputPath, outputFileName), ',', 0, listOfObjectCoordinates, listOfLbsParameters);
    }
}