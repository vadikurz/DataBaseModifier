using System;
using ConsoleService.Models;
using NUnit.Framework;

namespace ConsoleService.Unit.Tests;

public class PointTests
{
    private DateTime timeStamp = new (2023, 3, 20, 13, 58, 5);
    private double Lat = 53.96756232773556;
    private double Lng = 27.4907966635347;
    private int NumberOfSatellites = 4;
    private int Mcc = 257;
    private int Mnc = 2;
    private int Lac = 21239;
    private int Cid = 21616;

    [Test]
    [TestCase("03/20/2023 13:58:05,53.96756232773556,27.4907966635347,4,257,2,21239,21616")]
    public void Parse_CorrectString_ReturnsParsedPoint(string testString)
    {
        var result = Point.TryParse(testString, out var resultPoint);

        Assert.IsTrue(result);
        Assert.AreEqual(timeStamp, resultPoint.Time);
        Assert.AreEqual(timeStamp.Kind, resultPoint.Time.Kind);
        Assert.AreEqual(Lat, resultPoint.Coords.Lat);
        Assert.AreEqual(Lng, resultPoint.Coords.Lng);
        Assert.AreEqual(NumberOfSatellites, resultPoint.Sat);
        Assert.AreEqual(Mcc, resultPoint.Lbs.Mcc);
        Assert.AreEqual(Mnc, resultPoint.Lbs.Mnc);
        Assert.AreEqual(Lac, resultPoint.Lbs.Lac);
        Assert.AreEqual(Cid, resultPoint.Lbs.Cid);
    }

    [Test]
    [TestCase("53.96756232773556,27.4907966635347,03/19/2023 14:22:06,5,257,2,21239,21616")]
    public void Parse_InvalidString_ReturnsFalse(string line)
    {
        var result = Point.TryParse(line, out var resultPoint);

        Assert.IsFalse(result);
    }
}