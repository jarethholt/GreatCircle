namespace GreatCircle.Tests;

public class GreatCirclePathTests
{
    private readonly string degreeSymbol = "\u00B0";

    /// <summary>
    /// Check that a <c>NotImplementedException</c> is thrown for a path starting at a pole.
    /// </summary>
    [Fact]
    public void Path_InitialPoint_Polar()
    {
        Assert.Throws<NotImplementedException>(
            () => new GreatCirclePath(Coordinate.NorthPole, 10));
    }

    /// <summary>
    /// Check that a <c>NotImplementedException</c> is thrown for a purely northward path.
    /// </summary>
    [Fact]
    public void Path_Azimuth_Northward()
    {
        Assert.Throws<NotImplementedException>(
            () => new GreatCirclePath(Coordinate.Origin, 0));
    }

    /// <summary>
    /// Check that a <c>NotImplementedException</c> is thrown for a purely southward path.
    /// </summary>
    [Fact]
    public void Path_Azimuth_Southward()
    {
        Assert.Throws<NotImplementedException>(
            () => new GreatCirclePath(Coordinate.Origin, 180));
    }

    /// <summary>
    /// Check that a <c>NotImplementedException</c> is thrown for a nearly-northward path.
    /// </summary>
    [Fact]
    public void Path_Azimuth_NorthwardWrapped()
    {
        Assert.Throws<NotImplementedException>(
            () => new GreatCirclePath(Coordinate.Origin, 360 - 1e-8));
    }

    /// <summary>
    /// Check that the default string formatting is correct.
    /// </summary>
    [Fact]
    public void String_DefaultFormat()
    {
        double latitude = 10;
        double longitude = 70;
        double azimuth = 30;
        Assert.Equal(
            $"{latitude:F2}{degreeSymbol} N, {longitude:F2}{degreeSymbol} E; "
            + $"{azimuth:F2}{degreeSymbol}",
            new GreatCirclePath(latitude, longitude, azimuth).ToString());
    }

    /// <summary>
    /// Check that a custom string formatting is correct.
    /// </summary>
    [Fact]
    public void String_CustomFormat()
    {
        double latitude = 10;
        double longitude = 70;
        double azimuth = 30;
        Assert.Equal(
            $"{latitude:F0}{degreeSymbol} N, {longitude:F0}{degreeSymbol} E; "
            + $"{azimuth:F0}{degreeSymbol}",
            new GreatCirclePath(latitude, longitude, azimuth).ToString("F0"));
    }
}
