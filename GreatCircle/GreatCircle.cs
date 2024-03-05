﻿// Class providing the functionality of a great circle path.

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
     * initialAzimuth: Azimuth of the path at the initial point
     *   in degrees clockwise from northward, also normalized to [-180, 180).
     *   NB: At the moment purely meridional paths are not implemented, so
     *   this field is constrained to (-180, 0) and (0, 180).
     */
    private const double _degToRad = Math.PI / 180;
    private const double _radToDeg = 180.0 / Math.PI;

    private readonly Coordinates.Coordinate _initialCoordinate;
    private readonly double _initialAzimuth;

    public readonly Coordinates.Coordinate InitialCoordinate
    {
        get => _initialCoordinate;
        init => _initialCoordinate = value;
    }

    public readonly double InitialAzimuth
    {
        get => _initialAzimuth;
        init
        {
            // Put the azimuth in the range [-180, 180)
            value %= 360;
            value = value < 180 ? value : value - 360;
            // Check that the azimuth is not meridional
            if (
                MyMathUtils.MyMath.IsCloseTo(value, 0)
                || MyMathUtils.MyMath.IsCloseTo(value, 0)
                || MyMathUtils.MyMath.IsCloseTo(value, 0))
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
    {
        InitialCoordinate = new(initialLatitude, initialLongitude);
        InitialAzimuth = initialAzimuth;
    }

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

}
