using System;
using System.Globalization;
using ConsoleService.Models;
using NUnit.Framework;

namespace LBSTests;

public class PointTests
{
    [Test]
    public void Parse_CorrectString_ReturnsParsedPoint()
    {
        var timeStamp = new DateTimeOffset(2023, 3, 19, 14, 22, 06, new TimeSpan(3, 0, 0));
        var latitude = 53.96756232773556;
        var longitude = 27.4907966635347;
        var numberOfSatellites = 5;
        var mcc = 257;
        var mnc = 2;
        var lacTacNid = 21239;
        var cid = 21616;
        var timeStampString = timeStamp.ToString(CultureInfo.InvariantCulture);
        var latitudeString = latitude.ToString(CultureInfo.InvariantCulture);
        var longitudeString = longitude.ToString(CultureInfo.InvariantCulture);
        var line =
            $"{timeStampString},{latitudeString},{longitudeString},{numberOfSatellites},{mcc},{mnc},{lacTacNid},{cid}";

        var result = Point.TryParse(line, out var resultPoint);

        Assert.IsTrue(result);
        Assert.AreEqual(timeStamp, resultPoint.TimeStamp);
        Assert.AreEqual(latitude, resultPoint.Latitude);
        Assert.AreEqual(longitude, resultPoint.Longitude);
        Assert.AreEqual(numberOfSatellites, resultPoint.NumberOfSatellites);
        Assert.AreEqual(mcc, resultPoint.Mcc);
        Assert.AreEqual(mnc, resultPoint.Mnc);
        Assert.AreEqual(lacTacNid, resultPoint.LacTacNid);
        Assert.AreEqual(cid, resultPoint.Cid);
    }

    [Test]
    [TestCase("53.96756232773556,27.4907966635347,03/19/2023 14:22:06 +03:00,5,257,2,21239,21616")]
    public void Parse_InvalidString_ReturnsFalse(string line)
    {
        var result = Point.TryParse(line, out var resultPoint);

        Assert.IsFalse(result);
    }
}