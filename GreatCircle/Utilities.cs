// Utility for checking whether doubles are close within a tolerance

namespace GreatCircle;

public static class AngleUtilities
{
    public const double degToRad = Math.PI / 180;
    public const double radToDeg = 180.0 / Math.PI;
    public const char Degree = '\u00B0';
    public const char Minute = '\'';
    public const char Second = '"';

    /// <summary>
    /// Simultaneously compute the sine and cosine of an angle given in degrees.
    /// </summary>
    /// <param name="angleInDegrees">The angle to compute sine and cosine of in degrees.</param>
    /// <returns>The sine and cosine of the angle.</returns>
    public static (double, double) SinCosWithDegrees(double angleInDegrees)
        => Math.SinCos(angleInDegrees * degToRad);

    /// <summary>
    /// Calculate the quadrant-accurate arctangent of y/x in degrees.
    /// </summary>
    /// <param name="y">The numerator in the arctangent.</param>
    /// <param name="x">The denominator in the arctangent.</param>
    /// <returns>The angle in the correct quadrant in degrees.</returns>
    public static double Atan2ToDegrees(double y, double x)
        => Math.Atan2(y, x) * radToDeg;

    /// <summary>
    /// Calculate the quadrant-accurate arctangent of y/x in degrees.
    /// Simultaneously, calculate the sine and cosine of that angle using y and x directly.
    /// </summary>
    /// <param name="y">The numerator in the arctangent.</param>
    /// <param name="x">The denominator in the arctangent.</param>
    /// <returns>The sine, cosine, and angle in degrees.</returns>
    public static (double, double, double) SinCosAngleFromAtan2(double y, double x)
    {
        double angle = Atan2ToDegrees(y, x);
        double norm = Math.Sqrt(y * y + x * x);
        double sin = y / norm;
        double cos = x / norm;
        return (sin, cos, angle);
    }

    /// <summary>
    /// Normalize a value to conform to azimuth conventions. The azimuth is typically measured
    /// clockwise from due North, i.e. in the range (0, 360].
    /// </summary>
    /// <param name="value">The value to normalize.</param>
    /// <returns>An equivalent value in the range (0, 360].</returns>
    public static double NormalizeToAzimuth(double value)
    {
        double result = value % 360;
        result = (result < 0) ? result + 360 : result;
        result = (result == 0) ? 360 : result;
        return result;
    }

    /// <summary>
    /// Normalize a value to conform to longitude conventions. Longitudes are typically given
    /// between 180 W and 180 E, i.e. in the range [-180, 180).
    /// </summary>
    /// <param name="value">The value to normalize.</param>
    /// <returns>An equivalent value in the range [-180, 180).</returns>
    public static double NormalizeToLongitude(double value)
    {
        double lon = NormalizeToAzimuth(value);
        return lon < 180 ? lon : lon - 360;
    }

    /// <summary>
    /// Normalize a value to conform to latitude conventions. Latitudes should be between
    /// -90 and 90 in degrees. Values above and below these are expected to wrap around
    /// the poles, so for example 110 = 90 + 20 => 90 - 20 = 70.
    /// </summary>
    /// <param name="lat">The value to normalize.</param>
    /// <returns>An equivalent value in the range [-90, 90].</returns>
    public static double NormalizeToLatitude(double value)
    {
        double lat = NormalizeToLongitude(value);
        int sign = Math.Sign(lat);
        return Math.Abs(lat) <= 90 ? lat : sign * 180 - lat;
    }

    public static (int, double) DegreesToDegreeMinutes(double angle)
    {
        int degrees = (int)Math.Truncate(angle);
        double minutes = Math.Abs((angle - degrees) * 60);
        return (degrees, minutes);
    }

    public static (int, int, double) DegreesToDegreeSeconds(double angle)
    {
        int degrees = (int)Math.Truncate(angle);
        int minutes = (int)Math.Abs((angle - degrees) * 60);
        double seconds = (Math.Abs(angle - degrees) * 60 - minutes) * 60;
        return (degrees, minutes, seconds);
    }

    public static string ToStringNearestDegree(double angle)
        => $"{(int)angle}{Degree}";

    public static string ToStringNearestMinute(double angle)
    {
        (int degrees, double minutes) = DegreesToDegreeMinutes(angle);
        return $"{degrees}{Degree}{(int)minutes}{Minute}";
    }

    public static string ToStringNearestSecond(double angle)
    {
        (int degrees, int minutes, double seconds) = DegreesToDegreeSeconds(angle);
        return $"{degrees}{Degree}{minutes}{Minute}{(int)seconds}{Second}";
    }
}

/// <summary>Default tolerance values for the DoubleUtilities.*Close functions.</summary>
public readonly struct CloseToToleranceDefaults
{
    /// <summary>The tolerance relative to the magnitude of the inputs.</summary>
    public const double RelativeTolerance = 1e-6;
    /// <summary>The tolerance independent of the input values.</summary>
    public const double AbsoluteTolerance = 1e-8;
}

public static class CloseToUtilities
{
    /// <summary>
    /// Determine whether a value is sufficiently close to a target.
    /// </summary>
    /// <param name="value">The value to test.</param>
    /// <param name="target">The target to check against.</param>
    /// <param name="relativeTolerance">The tolerance relative to the size of target.</param>
    /// <param name="absoluteTolerance">The absolute tolerance to check.</param>
    /// <returns>Whether the difference between value and target is within the tolerance.</returns>
    /// <remarks>
    /// The total tolerance is calculated as
    /// (absolute tolerance + relative tolerance * magnitude of target)
    /// so that the target is treated asymmetrically as the "true" value.
    /// </remarks>
    public static bool IsCloseTo(
        double value,
        double target,
        double relativeTolerance = CloseToToleranceDefaults.RelativeTolerance,
        double absoluteTolerance = CloseToToleranceDefaults.AbsoluteTolerance
    ) => Math.Abs(value - target) <= absoluteTolerance + relativeTolerance * Math.Abs(target);

    /// <summary>
    /// Determine whether two values are sufficiently close to each other.
    /// </summary>
    /// <param name="value1">First value to test.</param>
    /// <param name="value2">Second value to test.</param>
    /// <param name="relativeTolerance">The tolerance relative to the size of the values.</param>
    /// <param name="absoluteTolerance">The absolute tolerance to check.</param>
    /// <returns></returns>
    /// <remarks>
    /// The total tolerance is calculated as
    /// (absolute tolerance + relative tolerance * (mean magnitude of value1 and value2)
    /// so that both values are treated symmetrically; neither is the "true" value.
    /// </remarks>
    public static bool AreClose(
        double value1,
        double value2,
        double relativeTolerance = CloseToToleranceDefaults.RelativeTolerance,
        double absoluteTolerance = CloseToToleranceDefaults.AbsoluteTolerance
    ) => (
        Math.Abs(value1 - value2)
        <= absoluteTolerance + relativeTolerance * 0.5 * (Math.Abs(value1) + Math.Abs(value2))
    );
}
