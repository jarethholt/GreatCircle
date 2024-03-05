// Utility for checking whether doubles are close within a tolerance

namespace GreatCircle;

public struct ToleranceDefaults
{
    public const double relativeTolerance = 1e-6;
    public const double absoluteTolerance = 1e-6;
}

public static class Utilities
{
    public const double degToRad = Math.PI / 180;
    public const double radToDeg = 180.0 / Math.PI;

    // Functions to determine whether two doubles are close
    public static bool IsCloseTo(
        double value,
        double target,
        double relativeTolerance = ToleranceDefaults.relativeTolerance,
        double absoluteTolerance = ToleranceDefaults.absoluteTolerance)
    {
        /* Determine whether a given value is close to the target.
         * Based on numpy.isclose, comparing the absolute error to a combined tolerance.
         */
        return Math.Abs(value - target) <= absoluteTolerance + relativeTolerance * Math.Abs(target);
    }

    public static bool AreClose(
        double value1,
        double value2,
        double relativeTolerance = ToleranceDefaults.relativeTolerance,
        double absoluteTolerance = ToleranceDefaults.absoluteTolerance)
    {
        /* Determine whether two values are close to each other.
         * Based on numpy.isclose but treats value1 and value2 symmetrically.
         */
        double scale = 0.5 * (Math.Abs(value1) + Math.Abs(value2));
        return Math.Abs(value1 - value2) <= absoluteTolerance + relativeTolerance * scale;
    }

    // Additional useful math functions
    public static (double, double) SinCosFromTanArgs(double y, double x)
    {
        double norm = Math.Sqrt(y * y + x * x);
        return (y / norm, x / norm);
    }

    public static (double, double) SinCosWithDegrees(double angleInDegrees)
        => Math.SinCos(angleInDegrees * degToRad);

    public static double Atan2ToDegrees(double y, double x)
        => Math.Atan2(y, x) * radToDeg;

    public static (double, double, double) SinCosAngleFromAtan2(double y, double x)
    {
        (double sin, double cos) = SinCosFromTanArgs(y, x);
        double angle = Atan2ToDegrees(y, x);
        return (sin, cos, angle);
    }

}
