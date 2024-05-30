using Newtonsoft.Json.Linq;

namespace GreatCircle.Tests;

public class AnglesTests
{
    /// <summary>
    /// Check that degrees and minutes are correctly calculated for a positive angle.
    /// </summary>
    [Fact]
    public void DegreesToDegreeMinutes_Positive()
    {
        double angle = 120.87;
        int expectedDegrees = 120;
        double expectedMinutes = 52.2;
        (int actualDegrees, double actualMinutes) = AngleUtilities.DegreesToDegreeMinutes(angle);
        Assert.Equal(expectedDegrees, actualDegrees);
        Assert.Equal(expectedMinutes, actualMinutes, precision: 2);
    }

    /// <summary>
    /// Check that degrees and minutes are correctly calculated for a negative angle.
    /// </summary>
    [Fact]
    public void DegreesToDegreeMinutes_Negative()
    {
        double angle = -120.87;
        int expectedDegrees = -120;
        double expectedMinutes = 52.2;
        (int actualDegrees, double actualMinutes) = AngleUtilities.DegreesToDegreeMinutes(angle);
        Assert.Equal(expectedDegrees, actualDegrees);
        Assert.Equal(expectedMinutes, actualMinutes, precision: 2);
    }

    /// <summary>
    /// Check that degrees, minutes, and seconds are correctly calculated for a positive angle.
    /// </summary>
    [Fact]
    public void DegreesToDegreeSeconds_Positive()
    {
        double angle = 120.87;
        int expectedDegrees = 120;
        int expectedMinutes = 52;
        double expectedSeconds = 12;
        (int actualDegrees, int actualMinutes, double actualSeconds)
            = AngleUtilities.DegreesToDegreeSeconds(angle);
        Assert.Equal(expectedDegrees, actualDegrees);
        Assert.Equal(expectedMinutes, actualMinutes);
        Assert.Equal(expectedSeconds, actualSeconds, precision: 2);
    }

    /// <summary>
    /// Check that degrees, minutes, and seconds are correctly calculated for a negative angle.
    /// </summary>
    [Fact]
    public void DegreesToDegreeSeconds_Negative()
    {
        double angle = -120.87;
        int expectedDegrees = -120;
        int expectedMinutes = 52;
        double expectedSeconds = 12;
        (int actualDegrees, int actualMinutes, double actualSeconds)
            = AngleUtilities.DegreesToDegreeSeconds(angle);
        Assert.Equal(expectedDegrees, actualDegrees);
        Assert.Equal(expectedMinutes, actualMinutes);
        Assert.Equal(expectedSeconds, actualSeconds, precision: 2);
    }

    /// <summary>
    /// Check that large positive angles are normalized to azimuth correctly.
    /// </summary>
    [Fact]
    public void NormalizeToAzimuth_PositiveAngle()
    {
        double angle = 385;
        double expected = 25;  // angle - 360
        double actual = AngleUtilities.NormalizeToAzimuth(angle);
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Check that negative angles are normalized to azimuth correctly.
    /// </summary>
    [Fact]
    public void NormalizeToAzimuth_NegativeAngle()
    {
        double angle = -20;
        double expected = 340;  // angle + 360
        double actual = AngleUtilities.NormalizeToAzimuth(angle);
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Check that large positive angles are normalized to longitude correctly.
    /// </summary>
    [Fact]
    public void NormalizeToLongitude_PositiveAngle()
    {
        double angle = 200;
        double expected = -160;  // angle - 360
        double actual = AngleUtilities.NormalizeToLongitude(angle);
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Check that negative angles are normalized to longitude correctly.
    /// </summary>
    [Fact]
    public void NormalizeToLongitude_NegativeAngle()
    {
        double angle = -200;
        double expected = 160;  // angle + 360
        double actual = AngleUtilities.NormalizeToLongitude(angle);
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Check that a moderately large positive angle is normalized to latitude correctly.
    /// </summary>
    [Fact]
    public void NormalizeToLatitude_PositiveAngle()
    {
        double angle = 100;
        double expected = 80;  // 180 - angle
        double actual = AngleUtilities.NormalizeToLatitude(angle);
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Check that a very large positive angle is normalized to latitude correctly.
    /// </summary>
    [Fact]
    public void NormalizeToLatitude_LargePositiveAngle()
    {
        double angle = 200;
        double expected = -20;  // 180 - angle
        double actual = AngleUtilities.NormalizeToLatitude(angle);
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Check that a moderately large negative angle is normalized to latitude correctly.
    /// </summary>
    [Fact]
    public void NormalizeToLatitude_NegativeAngle()
    {
        double angle = -100;
        double expected = -80;  // -180 - angle
        double actual = AngleUtilities.NormalizeToLatitude(angle);
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Check that a very large negative angle is normalized to latitude correctly.
    /// </summary>
    [Fact]
    public void NormalizeToLatitude_LargeNegativeAngle()
    {
        double angle = -200;
        double expected = 20;  // -180 - angle
        double actual = AngleUtilities.NormalizeToLatitude(angle);
        Assert.Equal(expected, actual);
    }
}

public class CloseToUtilitiesTests
{
    /// <summary>
    /// Check that IsCloseTo returns True for a value near the target.
    /// </summary>
    [Fact]
    public void IsCloseTo_NearTarget()
    {
        Assert.True(CloseToUtilities.IsCloseTo(1 - 1e-6, 1));
    }

    /// <summary>
    /// Check that IsCloseTo returns False for a value far from the target.
    /// </summary>
    [Fact]
    public void IsCloseTo_FarFromTarget()
    {
        Assert.False(CloseToUtilities.IsCloseTo(1 - 2.1e-6, 1));
    }

    /// <summary>
    /// Check that IsCloseTo returns True for a value near a target of 0.
    /// </summary>
    [Fact]
    public void IsCloseTo_NearZero()
    {
        Assert.True(CloseToUtilities.IsCloseTo(1e-8, 0));
    }

    /// <summary>
    /// Check that IsCloseTo returns False for a value far from a target of 0.
    /// </summary>
    [Fact]
    public void IsCloseTo_FarFromZero()
    {
        Assert.False(CloseToUtilities.IsCloseTo(1.1e-8, 0));
    }

    /// <summary>
    /// Check that AreClose returns True for two values close to each other.
    /// </summary>
    [Fact]
    public void AreClose_NearEachOther()
    {
        Assert.True(CloseToUtilities.AreClose(1 - 5e-7, 1 + 5e-7));
    }

    /// <summary>
    /// Check that AreClose returns False for two values far from each other.
    /// </summary>
    [Fact]
    public void AreClose_FarFromEachOther()
    {
        Assert.False(CloseToUtilities.AreClose(1 - 5.1e-7, 1 + 5.1e-7));
    }

    /// <summary>
    /// Check that AreClose returns True for two values near zero close to each other.
    /// </summary>
    [Fact]
    public void AreClose_NearZero_True()
    {
        Assert.True(CloseToUtilities.AreClose(5e-9, -5e-9));
    }

    /// <summary>
    /// Check that AreClose returns False for two values near zero far from each other.
    /// </summary>
    [Fact]
    public void AreClose_NearZero_False()
    {
        Assert.False(CloseToUtilities.AreClose(5.1e-9, -5e-9));
    }
}