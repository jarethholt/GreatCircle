// Class providing the functionality of a great circle path.

namespace GreatCircle;

public readonly struct GreatCirclePath
{
    /* Representation of a generic (non-polar) great circle path.
     * 
     * A great circle path given by an initial point and an azimuth.
     * Points along the path are then parameterized by a given central
     * angle from the initial point.
     * 
     * Properties
     * ------
     * InitialCoordinate: The initial point as a Coordinate object.
     *   NB: At the moment we cannot represent paths through the poles,
     *   so this Coordinate cannot be polar.
     * InitialAzimuth: Azimuth of the path at the initial point
     *   in degrees clockwise from northward, also normalized to [-180, 180).
     *   NB: At the moment purely meridional paths are not implemented, so
     *   this field is constrained to (-180, 0) and (0, 180).
     */

    private readonly Coordinate _initialCoordinate;
    private readonly double _initialAzimuth;
    private readonly double _nodeAzimuth;
    private readonly double _sinNodeAzi;
    private readonly double _cosNodeAzi;
    private readonly double _tanNodeAzi;
    private readonly double _nodeAngle;
    private readonly double _nodeLongitude;

    public readonly Coordinate InitialCoordinate
    {
        get => _initialCoordinate;
        init
        {
            // Check that the coordinate is not a pole
            if (value.IsAPole)
                throw new NotImplementedException(
                    "Paths through the poles have not been implemented yet");
            _initialCoordinate = value;
        }
    }

    public readonly double InitialLatitude => InitialCoordinate.Latitude;
    public readonly double InitialLongitude => InitialCoordinate.Longitude;

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

    public readonly double NodeAzimuth => _nodeAzimuth;
    public readonly double NodeAngle => _nodeAngle;
    public readonly double NodeLongitude => _nodeLongitude;

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

    public GreatCirclePath(
        double initialLatitude, double initialLongitude, double initialAzimuth)
        : this(new Coordinate(initialLatitude, initialLongitude), initialAzimuth) { }

    public override string ToString() => ToString("F2");

    public string ToString(string fmt)
    {
        if (string.IsNullOrEmpty(fmt))
            fmt = "F2";

        string coordString = InitialCoordinate.ToString(fmt);
        string aziFormat = string.Format(
            "heading {0}0:{3}{1}{2}",
            "{",
            "}",
            Coordinate.degreeSymbol,
            fmt);
        string aziString = string.Format(aziFormat, InitialAzimuth);
        return $"{coordString}; {aziString}";
    }

    public (double, double, double) DisplaceByAngle(double angle)
    {
        (double sinAngle, double cosAngle)
            = Utilities.SinCosWithDegrees(angle + _nodeAngle);
        double sinLat = _cosNodeAzi * sinAngle;
        double cosLat = Math.Sqrt(1 - sinLat * sinLat);
        double latitude = Utilities.Atan2ToDegrees(sinLat, cosLat);

        double lonDiff = Utilities.Atan2ToDegrees(_sinNodeAzi * sinAngle, cosAngle);
        double longitude = lonDiff + _nodeLongitude;

        double azimuth = Utilities.Atan2ToDegrees(_sinNodeAzi / _cosNodeAzi, cosAngle);
        return (latitude, longitude, azimuth);
    }

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

    public static (GreatCirclePath, double) PathAndAngleBetweenPoints(
        Coordinate initial, Coordinate final)
    {
        // Need to calculate the correct initial azimuth
        (double sinLonDiff, double cosLonDiff)
            = Utilities.SinCosWithDegrees(final.Longitude - initial.Longitude);
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
