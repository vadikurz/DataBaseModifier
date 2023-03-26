using System.Text;

namespace Common.Models;

public record Lbs(int Mcc, int Mnc, int Lac, int Cid)
{
    public override string ToString()
    {
        const char delimiter = ',';

        var builder = new StringBuilder();
        
        builder.Append(Mcc);
        builder.Append(delimiter);
        builder.Append(Mnc);
        builder.Append(delimiter);
        builder.Append(Lac);
        builder.Append(delimiter);
        builder.Append(Cid);

        return builder.ToString();
    }
}