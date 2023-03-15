namespace TestDataGenerator.Models;

public class Point
{
    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public Point(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public static Point Parse(string point)
    {
        var delimiter = ",";

        var arrayOfCoordinates = point.Split(delimiter);

        double.TryParse(arrayOfCoordinates[0], out var latitude);
        double.TryParse(arrayOfCoordinates[1], out var longitude);

        return new Point(latitude, longitude);
    }

    public override string ToString() => $"{Latitude},{Longitude}";
}