// Class describing a coordinate (latitude/longitude) on the sphere.
namespace GreatCircle;

/// <summary>
/// Structure representing a location (latitude and longitude coordinates) on a sphere.
/// </summary>
public readonly record struct Coordinate(double Latitude, double Longitude)
{
    /// <summary>The latitude of the coordinate in degrees N.</summary>
    /// <remarks>
    /// Any value can be provided but will be converted to the range [-90, 90]
    /// by assuming that the latitude wraps around the poles. See
    /// AngleUtilities.NormalizeToLatitude for details.
    /// </remarks>
    public readonly double Latitude = AngleUtilities.NormalizeToLatitude(Latitude);

    /// <summary>The longitude of the coordinate in degrees E.</summary>
    /// <remarks>
    /// Any value can be given as the longitude;
    /// however, it will be converted to the range [-180, 180).
    /// Note that longitude is irrelevant for points at the poles
    /// (Latitude = -90 or 90) but must still be provided.
    /// </remarks>
    public readonly double Longitude = AngleUtilities.NormalizeToLongitude(Longitude);

    /// <summary>Format the coordinate as a string.</summary>
    /// <returns>A string representation of the coordinate.</returns>
    /// <remarks>
    /// Calls the more general <c>ToString(string fmt)</c> method
    /// with a default value <c>fmt="F2"</c>.
    /// </remarks>
    public override string ToString() => ToString("F2");

    /// <summary>Format the coordinate as a string with given formatting.</summary>
    /// <param name="fmt">The type of floating-point format to use.</param>
    /// <returns>A string representation of the coordinate.</returns>
    /// <remarks>
    /// The <c>fmt</c> is used for the representation of both longitude and latitude.
    /// This format string is automatically passed in when providing a <c>Coordinate</c>
    /// as a parameter in string interpolation:
    /// <code>
    ///     Coordinate coordinate = new(75, 10);
    ///     string actual = $"{coordinate:F2}";
    ///     string expected = "75.00° N, 10.00° E";
    /// </code>
    /// </remarks>
    public string ToString(string fmt)
    {
        if (string.IsNullOrEmpty(fmt))
            fmt = "F2";
        string northOrSouth = Latitude >= 0 ? "N" : "S";
        string eastOrWest = Longitude >= 0 ? "E" : "W";
        string messageFormat =
            $"{{0:{fmt}}}{AngleUtilities.Degree} {northOrSouth}, "
            + $"{{1:{fmt}}}{AngleUtilities.Degree} {eastOrWest}";
        return string.Format(messageFormat, Math.Abs(Latitude), Math.Abs(Longitude));
    }

    /// <summary>
    /// Coordinate representing the North Pole.
    /// </summary>
    public static Coordinate NorthPole => new(90, 0);

    /// <summary>
    /// Coordinate representing the South Pole.
    /// </summary>
    public static Coordinate SouthPole => new(-90, 0);

    /// <summary>
    /// Coordinate representing a "default" origin at 0 N, 0 W.
    /// </summary>
    public static Coordinate Origin => new(0, 0);

    /// <summary>
    /// Coordinate representing the antipode (opposite) of the current point.
    /// </summary>
    /// <returns>The antipodal coordinate.</returns>
    public Coordinate Antipode() => new(-Latitude, Longitude + 180);

    /// <summary>
    /// Test if this coordinate is a polar point (north or south pole).
    /// </summary>
    /// <returns>
    /// Whether the latitude is sufficiently close to 90 or -90.
    /// See <see cref="CloseToUtilities.IsCloseTo"/> for details.
    /// </returns>
    public bool IsAPole() =>
        CloseToUtilities.IsCloseTo(Math.Abs(Latitude), 90);

    /// <summary>
    /// Whether this coordinate is sufficiently close to another.
    /// </summary>
    /// <param name="other">The other coordinate to compare to.</param>
    /// <returns>
    /// Whether or not the latitude and longitude of these coordinates are sufficiently close.
    /// See <see cref="CloseToUtilities.AreClose"/> for details.
    /// </returns>
    public bool IsCloseTo(Coordinate other)
    {
        bool result = CloseToUtilities.AreClose(Latitude, other.Latitude);
        if (!result)
            return result;

        // Polar points can have any longitude, so don't check longitude equality!
        if (IsAPole() || other.IsAPole())
            return result;

        // Check longitude equality by normalizing the longitude difference to the range [-180, 180)
        double lonDiff = AngleUtilities.NormalizeToLongitude(other.Longitude - Longitude);
        result = CloseToUtilities.IsCloseTo(lonDiff, 0);
        return result;
    }

    /// <summary>
    /// Test whether a coordinate is (nearly) antipodal to this one.
    /// </summary>
    /// <param name="other">The other coordinate to check.</param>
    /// <returns>Whether the other coordinate and the antipode of this one are close.</returns>
    public bool IsAntipodalTo(Coordinate other) =>
        Antipode().IsCloseTo(other);
}
