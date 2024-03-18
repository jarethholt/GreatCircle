// Utility for checking whether doubles are close within a tolerance

namespace GreatCircle;

public static class Angles
{
    public const char Degree = '\u00B0';
    public const char Minute = '\'';
    public const char Second = '"';

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

/// <summary>Default tolerance values for the Utilities.*Close functions.</summary>
public struct ToleranceDefaults
{
    /// <summary>The tolerance relative to the magnitude of the inputs.</summary>
    public const double relativeTolerance = 1e-6;
    /// <summary>The tolerance independent of the input values.</summary>
    public const double absoluteTolerance = 1e-8;
}

public static class Utilities
{
    public const double degToRad = Math.PI / 180;
    public const double radToDeg = 180.0 / Math.PI;

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
        double relativeTolerance = ToleranceDefaults.relativeTolerance,
        double absoluteTolerance = ToleranceDefaults.absoluteTolerance
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
        double relativeTolerance = ToleranceDefaults.relativeTolerance,
        double absoluteTolerance = ToleranceDefaults.absoluteTolerance
    ) => (
        Math.Abs(value1 - value2)
        <= absoluteTolerance + relativeTolerance * 0.5 * (Math.Abs(value1) + Math.Abs(value2))
    );

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
        double norm = Math.Sqrt(y * y + x * x);
        double sin = y / norm;
        double cos = x / norm;
        double angle = Atan2ToDegrees(y, x);
        return (sin, cos, angle);
    }

}
