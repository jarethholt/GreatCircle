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
     * initialAzimuth: Azimuth of the path at the initial point
     *   in degrees clockwise from northward, also normalized to [-180, 180).
     *   NB: At the moment purely meridional paths are not implemented, so
     *   this field is constrained to (-180, 0) and (0, 180).
     */

    private readonly Coordinates.Coordinate _initialCoordinate;
    private readonly double _initialAzimuth;

    public readonly Coordinates.Coordinate InitialCoordinate
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
                MyMathUtils.MyMath.IsCloseTo(value, 0)
                || MyMathUtils.MyMath.IsCloseTo(value, 180)
                || MyMathUtils.MyMath.IsCloseTo(value, 360))
                throw new NotImplementedException(
                    "Purely meridional paths (azimuth = 0 or 180) have not been implemented yet");
            _initialAzimuth = value;
        }
    }

    public GreatCirclePath(Coordinates.Coordinate coordinate, double initialAzimuth)
    {
        InitialCoordinate = coordinate;
        InitialAzimuth = initialAzimuth;
    }

    public GreatCirclePath(double initialLatitude, double initialLongitude, double initialAzimuth)
        : this(new Coordinates.Coordinate(initialLatitude, initialLongitude), initialAzimuth) { }

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
            Coordinates.Coordinate.degreeSymbol,
            fmt);
        string aziString = string.Format(aziFormat, InitialAzimuth);
        return $"{coordString}; {aziString}";
    }

    // Calculate the ascending node
    public double NodeAzimuth()
    {
        (double sinInitLat, double cosInitLat)
            = MyMathUtils.MyMath.SinCosWithDegrees(InitialLatitude);
        (double sinInitAzi, double cosInitAzi)
            = MyMathUtils.MyMath.SinCosWithDegrees(InitialAzimuth);
        double sinNodeAzi = sinInitAzi * cosInitLat;
        double cosNodeAzi = Math.Sqrt(1 - sinNodeAzi * sinNodeAzi);
        return MyMathUtils.MyMath.Atan2ToDegrees(sinNodeAzi, cosNodeAzi);
    }

    public double NodeCentralAngle()
    {
        (double sinInitLat, double cosInitLat)
            = MyMathUtils.MyMath.SinCosWithDegrees(InitialLatitude);
        (double sinInitAzi, double cosInitAzi)
           = MyMathUtils.MyMath.SinCosWithDegrees(InitialAzimuth);
        double y = sinInitLat / cosInitLat;
        double x = cosInitAzi;
        (double sinNodeCentralAngle, double cosNodeCentralAngle)
            = MyMathUtils.MyMath.SinCosFromTanArgs(y, x);
        return MyMathUtils.MyMath.Atan2ToDegrees(y, x);
    }

    public double NodeLongitude()
    {
        (double sinNodeAzi, double cosNodeAzi)
            = MyMathUtils.MyMath.SinCosWithDegrees(NodeAzimuth());
        (double sinNodeCentralAngle, double cosNodeCentralAngle)
            = MyMathUtils.MyMath.SinCosWithDegrees(NodeCentralAngle());
        double y = sinNodeAzi * sinNodeCentralAngle;
        double x = cosNodeCentralAngle;
        (double sinNodeLonDiff, double cosNodeLonDiff)
            = MyMathUtils.MyMath.SinCosFromTanArgs(y, x);
        double nodeLonDiff = MyMathUtils.MyMath.Atan2ToDegrees(y, x);
        return InitialLongitude - nodeLonDiff;
    }

    public double CalcLatitude(double centralAngle)
    {
        (double sinCentralAngle, double cosCentralAngle)
            = MyMathUtils.MyMath.SinCosWithDegrees(centralAngle + NodeCentralAngle());
        double nodeAzi = NodeAzimuth();
        (double sinNodeAzi, double cosNodeAzi)
            = MyMathUtils.MyMath.SinCosWithDegrees(nodeAzi);
        double sinLat = cosNodeAzi * sinCentralAngle;
        double cosLat = Math.Sqrt(1 - sinLat * sinLat);
        return MyMathUtils.MyMath.Atan2ToDegrees(sinLat, cosLat);
    }

    public double CalcLongitude(double centralAngle)
    {
        (double sinCentralAngle, double cosCentralAngle)
            = MyMathUtils.MyMath.SinCosWithDegrees(centralAngle + NodeCentralAngle());
        double nodeAzi = NodeAzimuth();
        (double sinNodeAzi, double cosNodeAzi)
            = MyMathUtils.MyMath.SinCosWithDegrees(nodeAzi);
        double y = sinNodeAzi * sinCentralAngle;
        double x = cosCentralAngle;
        double lonDiff = MyMathUtils.MyMath.Atan2ToDegrees(y, x);
        return lonDiff + NodeLongitude();
    }

    public double CalcAzimuth(double centralAngle)
    {
        (double sinCentralAngle, double cosCentralAngle)
            = MyMathUtils.MyMath.SinCosWithDegrees(centralAngle + NodeCentralAngle());
        double nodeAzi = NodeAzimuth();
        (double sinNodeAzi, double cosNodeAzi)
            = MyMathUtils.MyMath.SinCosWithDegrees(nodeAzi);
        double tanNodeAzi = sinNodeAzi / cosNodeAzi;
        return MyMathUtils.MyMath.Atan2ToDegrees(tanNodeAzi, cosCentralAngle);
    }
}
