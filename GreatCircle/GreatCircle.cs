// Class providing the functionality of a great circle path.
namespace GreatCircle;

/// <summary>
/// Structure representing the path of a great circle.
/// </summary>
/// <remarks>
/// The term "path" here is used to denote not just which great circle,
/// but also a direction of travel. For example, the equator is a great
/// circle, but the paths traveling east and west along the equator
/// are considered different.
/// <para>
/// Paths here also have designated starting points that are referenced
/// in calculations. In that sense, a great circle path starting at
/// 0 N, 0 E and one starting at 0 N, 180 W both traveling eastward
/// will also be considered different.
/// </para>
/// </remarks>
public readonly struct GreatCirclePath
{
    private readonly Coordinate _initialCoordinate;
    /// <summary>The starting location of the great circle path.</summary>
    /// <value>Starting <c>Coordinate</c>.</value>
    public readonly Coordinate InitialCoordinate
    {
        get => _initialCoordinate;
        init
        {
            // Check that the coordinate is not a pole
            if (value.IsAPole())
                throw new NotImplementedException(
                    "Paths through the poles have not been implemented yet");
            _initialCoordinate = value;
        }
    }

    private readonly double _initialAzimuth;
    /// <summary>The direction of the great circle path at the starting location.</summary>
    /// <value>Azimuth of path at <c>InitialCoordinate</c>.</value>
    /// <remarks>
    /// The convention is that the azimuth is measured in degrees clockwise from
    /// northward, in the range [0, 360). Thus we get the values northward = 0,
    /// eastward = 90, southward = 180, westward = 270.
    /// <para>
    /// Any value can be provided for the initial azimuth, but it will be converted
    /// internally to the range [0, 360).
    /// </para>
    /// <para>
    /// Purely polar paths are not currently implemented. This means that the azimuth
    /// should not be a multiple of 180 (purely northward or southward).
    /// </para>
    /// </remarks>
    public readonly double InitialAzimuth
    {
        get => _initialAzimuth;
        init
        {
            // Put the azimuth in the range [0, 360)
            value %= 360;
            // Check that the azimuth is not meridional
            if (
                Utilities.IsCloseTo(value, 0)
                || Utilities.IsCloseTo(value, 180)
                || Utilities.IsCloseTo(value, 360))
                throw new NotImplementedException(
                    "Purely meridional paths (azimuth = 0 or 180) "
                    + "have not been implemented yet");
            _initialAzimuth = value;
        }
    }

    /// <summary>Latitude of the <c>InitialCoordinate</c> in degrees N.</summary>
    public readonly double InitialLatitude => InitialCoordinate.Latitude;
    /// <summary>Longitude of the <c>InitialCoordinate</c> in degrees E.</summary>
    public readonly double InitialLongitude => InitialCoordinate.Longitude;

    private readonly double _nodeAzimuth;
    /// <summary>Azimuth of the great circle path at the ascending node.</summary>
    /// <value>Azimuth of path at the node.</value>
    /// <remarks>
    /// The ascending node is the point at which the great circle path crosses
    /// the equator going northward. Values of various parameters at the node
    /// are used extensively in other calculations.
    /// </remarks>
    public readonly double NodeAzimuth => _nodeAzimuth;

    private readonly double _nodeLongitude;
    /// <summary>Longitude of the ascending node.</summary>
    /// <value>Longitude of the node.</value>
    /// <remarks>
    /// The ascending node is the point at which the great circle path crosses
    /// the equator going northward. Values of various parameters at the node
    /// are used extensively in other calculations.
    /// </remarks>
    public readonly double NodeLongitude => _nodeLongitude;

    private readonly double _nodeAngle;
    /// <summary>
    /// Central angle between the ascending node and the <c>InitialCoordinate</c>.
    /// </summary>
    /// <value>Central angle between node and starting point.</value>
    /// <remarks>
    /// The ascending node is the point at which the great circle path crosses
    /// the equator going northward. Values of various parameters at the node
    /// are used extensively in other calculations.
    /// </remarks>
    public readonly double NodeAngle => _nodeAngle;

    private readonly double _sinNodeAzi;
    private readonly double _cosNodeAzi;
    private readonly double _tanNodeAzi;

    /// <summary>
    /// Construct a great circle path from a given coordinate and direction.
    /// </summary>
    /// <param name="coordinate">Coordinate representing the starting point.</param>
    /// <param name="initialAzimuth">
    /// Direction (azimuth) of the path at the starting point.
    /// </param>
    /// <remarks>
    /// Purely polar paths are not currently implemented. This means that the azimuth
    /// should not be a multiple of 180 (purely northward or southward).
    /// </remarks>
    public GreatCirclePath(Coordinate coordinate, double initialAzimuth)
    {
        InitialCoordinate = coordinate;
        InitialAzimuth = initialAzimuth;

        // Calculate the ascending node
        (double sinInitLat, double cosInitLat)
            = Utilities.SinCosWithDegrees(InitialLatitude);
        double tanInitLat = sinInitLat / cosInitLat;
        (double sinInitAzi, double cosInitAzi)
            = Utilities.SinCosWithDegrees(InitialAzimuth);
        _sinNodeAzi = sinInitAzi * cosInitLat;
        _cosNodeAzi = Math.Sqrt(1 - _sinNodeAzi * _sinNodeAzi);
        _tanNodeAzi = _sinNodeAzi / _cosNodeAzi;
        _nodeAzimuth = Utilities.Atan2ToDegrees(_sinNodeAzi, _cosNodeAzi);
        
        (double sinNodeAngle, double cosNodeAngle, _nodeAngle)
            = Utilities.SinCosAngleFromAtan2(tanInitLat, cosInitAzi);

        double nodeLonDiff = Utilities.Atan2ToDegrees(
            _sinNodeAzi * sinNodeAngle, cosNodeAngle);
        _nodeLongitude = InitialLongitude - nodeLonDiff;
    }

    /// <summary>
    /// Construct a great circle path from a latitude, longitude, and direction.
    /// </summary>
    /// <param name="initialLatitude">Latitude of the starting point in degrees N.</param>
    /// <param name="initialLongitude">Longitude of the starting point in degrees E.</param>
    /// <param name="initialAzimuth">
    /// Direction (azimuth) of the path at the starting point.</param>
    /// <remarks>
    /// Purely polar paths are not currently implemented. This means that the azimuth
    /// should not be a multiple of 180 (purely northward or southward).
    /// </remarks>
    public GreatCirclePath(
        double initialLatitude, double initialLongitude, double initialAzimuth)
        : this(new Coordinate(initialLatitude, initialLongitude), initialAzimuth) { }

    /// <summary>Represent this great circle path as a string.</summary>
    /// <returns>String representation of the path.</returns>
    /// <remarks>Calls <c>ToString(fmt)</c> with a default value <c>fmt = "F2"</c>.</remarks>
    public override string ToString() => ToString("F2");

    /// <summary>Format the great circle path as a string with given formatting.</summary>
    /// <param name="fmt">The type of floating-point format to use.</param>
    /// <returns>A string representation of the path.</returns>
    /// <remarks>
    /// The <c>fmt</c> is used for the representation of longitude, latitude, and azimuth.
    /// This format string is automatically passed in when providing a <c>GreatCirclePath</c>
    /// as a parameter in string interpolation:
    /// <code>
    ///     GreatCirclePath path = new(75, 10, 30);
    ///     string actual = $"{path:F2}";
    ///     string expected = "75.00° N, 10.00° E; 30.00°";
    /// </code>
    /// </remarks>
    public string ToString(string fmt)
    {
        if (string.IsNullOrEmpty(fmt))
            fmt = "F2";

        string coordString = InitialCoordinate.ToString(fmt);
        string aziFormat = $"{{0:{fmt}}}{Coordinate.degreeSymbol}";
        string aziString = string.Format(aziFormat, InitialAzimuth);
        return $"{coordString}; {aziString}";
    }

    /// <summary>
    /// Find the point on the path a given central angle from the starting point.
    /// </summary>
    /// <param name="angle">The central angle from the starting point in degrees.</param>
    /// <returns>The new <c>Coordinate</c> location and azimuth.</returns>
    public (Coordinate, double) DisplaceByAngle(double angle)
    {
        (double sinAngle, double cosAngle)
            = Utilities.SinCosWithDegrees(angle + _nodeAngle);
        double sinLat = _cosNodeAzi * sinAngle;
        double cosLat = Math.Sqrt(1 - sinLat * sinLat);
        double latitude = Utilities.Atan2ToDegrees(sinLat, cosLat);

        double lonDiff = Utilities.Atan2ToDegrees(_sinNodeAzi * sinAngle, cosAngle);
        double longitude = lonDiff + _nodeLongitude;

        double azimuth = Utilities.Atan2ToDegrees(_sinNodeAzi / _cosNodeAzi, cosAngle);
        return (new Coordinate(latitude, longitude), azimuth);
    }

    /// <summary>
    /// Find the point on the path at a given longitude.
    /// </summary>
    /// <param name="longitude">Longitude in degrees of the new point.</param>
    /// <returns>The latitude and azimuth of the path at that point in degrees.</returns>
    public (double, double) DisplaceToLongitude(double longitude)
    {
        (double sinLonDiff, double cosLonDiff)
            = Utilities.SinCosWithDegrees(longitude - _nodeLongitude);
        double latitude = Utilities.Atan2ToDegrees(sinLonDiff / _tanNodeAzi, 1);

        double scale = Math.Sqrt(1 - _cosNodeAzi * _cosNodeAzi * cosLonDiff * cosLonDiff);
        double azimuth = Utilities.Atan2ToDegrees(
            _tanNodeAzi,
            _sinNodeAzi * cosLonDiff / scale);
        return (latitude, azimuth);
    }

    /// <summary>
    /// Construct the great circle path between two points.
    /// </summary>
    /// <param name="initial">Starting point.</param>
    /// <param name="final">Ending point.</param>
    /// <returns>
    /// The <c>GreatCirclePath</c> including both points
    /// and the central angle between them in degrees.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// If the points are antipodal since the great circle path between them is not unique.
    /// </exception>
    /// <exception cref="NotImplementedException">
    /// If the points lie on a great circle passing through the poles.
    /// </exception>
    public static (GreatCirclePath, double) PathAndAngleBetweenPoints(
        Coordinate initial, Coordinate final)
    {
        // Need to check that the points are not antipodal, polar or meridional
        if (initial.IsAntipodalTo(final))
            throw new ArgumentException(
                "The given points are antipodal; "
                + "the great circle path between them is not unique");
        if (initial.IsAPole() || final.IsAPole())
            throw new NotImplementedException(
                "Great circle paths through the poles have not been implemented yet");

        double lonDiff = final.Longitude - initial.Longitude;
        if (Utilities.IsCloseTo(lonDiff % 180, 0) || Utilities.IsCloseTo(lonDiff % 180, 180))
            throw new NotImplementedException(
                "Meridional great circle paths have not been implemented yet");

        // Need to calculate the correct initial azimuth
        (double sinLonDiff, double cosLonDiff) = Utilities.SinCosWithDegrees(lonDiff);
        (double sinInitLat, double cosInitLat) = Utilities.SinCosWithDegrees(initial.Latitude);
        (double sinFinalLat, double cosFinalLat) = Utilities.SinCosWithDegrees(final.Latitude);
        double y = cosFinalLat * sinLonDiff;
        double x = cosInitLat * sinFinalLat - sinInitLat * cosFinalLat * cosLonDiff;
        double initialAzimuth = Utilities.Atan2ToDegrees(y, x);

        // Calculate distance between points
        y = Math.Sqrt(y * y + x * x);
        x = sinInitLat * sinFinalLat + cosInitLat * cosFinalLat * cosLonDiff;
        double angle = Utilities.Atan2ToDegrees(y, x);
        return (new GreatCirclePath(initial, initialAzimuth), angle);
    }

}
