using System.Globalization;
using System.Text;
using TestDataGenerator.Models;

namespace TestDataGenerator.Services;

public class CsvWriter
{
    public async Task Write(string outputPath,
        char delimiter, int numberOfSatellites, List<Point> coordinates, List<LbsParameters> lbsParametersList)
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;

        await using var streamWriter = File.CreateText(outputPath);

        var resultLine = new StringBuilder();

        var time = DateTimeOffset.Now;

        for (int i = 0; i < coordinates.Count; i++)
        {
            resultLine.Clear();

            time = time.AddSeconds(1);

            resultLine.Append(time);
            resultLine.Append(delimiter);

            resultLine.Append(coordinates[i]);
            resultLine.Append(delimiter);

            resultLine.Append(numberOfSatellites);
            resultLine.Append(delimiter);

            resultLine.Append(lbsParametersList[i]);

            await streamWriter.WriteLineAsync(resultLine);
        }
    }
}