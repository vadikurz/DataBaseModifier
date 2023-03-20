using System.Text;

namespace ConsoleService.Models;

public record Coordinates(double Lat, double Lng)
{
    public override string ToString()
    {
        const char delimiter = ',';
        
        var sb = new StringBuilder();

        sb.Append(Lat);
        sb.Append(delimiter);
        sb.Append(Lng);

        return sb.ToString();
    }
}
