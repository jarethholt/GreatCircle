using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GreatCircle.MyMath;

namespace GreatCircle.Coordinates;

public class Coordinates
{
    /* Class representing location (in latitude and longitude) on a sphere.
     * 
     * Attributes:
     * - Latitude: Latitude in degrees N. Must be between -90 and 90. If a value
     *   equal to -90 or 90 is set (the south or north poles) then the longitude
     *   is automatically set to 0.
     * - Longitude: Longitude in degrees E. Any value can be given but it will be
     *   normalized to the range [-180, 180).
     */

    private const string _degreeSymbol = "\u00B0";
    private double _latitude;
    private double _longitude;

    public double Latitude
    {
        get { return _latitude; }
        set
        {
            // Check that latitude is between -90 and 90
            if (Math.Abs(value) > 90)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    "The latitude must be between -90 and 90");
            }

            // If the latitude is -90 or 90, set the longitude to 0
            if (MyMath.MyMath.IsCloseTo(Math.Abs(value), 90))
            {
                _longitude = 0;
            }
            _latitude = value;
        }
    }

    public double Longitude
    {
        get { return _longitude; }
        set
        {
            // If the latitude is -90 or 90, keep longitude pinned at 0
            if (MyMath.MyMath.IsCloseTo(Math.Abs(Latitude), 90))
            {
                _longitude = 0;
                return;
            }

            // Place the value in the range [0, 360)
            value %= 360;
            // Place the value in the range [-180, 180)
            _longitude = value < 180 ? value : value - 360;
        }
    }

    public Coordinates(double latitude, double longitude)
    {
        Longitude = longitude;
        Latitude = latitude;
    }

    public bool IsAPole()
    {
        // Determine whether the coordinate should be considered a polar point.
        return MyMath.MyMath.IsCloseTo(Math.Abs(_latitude), 90);
    }

    public override string ToString()
    {
        // Pretty print the coordinates
        return ToString("F2");
    }

    public string ToString(string fmt)
    {
        if (string.IsNullOrEmpty(fmt))
        {
            fmt = "F2";
        }
        string northOrSouth = Latitude >= 0 ? "N" : "S";
        string eastOrWest = Longitude >= 0 ? "E" : "W";
        string messageFormat = string.Format(
            "{0}0:{2}{1}{3} {4}, {0}1:{2}{1}{3} {5}",
            "{",
            "}",
            fmt,
            _degreeSymbol,
            northOrSouth,
            eastOrWest);
        return string.Format(messageFormat, Math.Abs(Latitude), Math.Abs(Longitude));
    }

    public bool IsCloseTo(Coordinates other)
    {
        // Test if two coordinates are too close to consider distinct
        return 
            MyMath.MyMath.AreClose(Longitude, other.Longitude)
            && MyMath.MyMath.AreClose(Latitude, other.Latitude);
    }

    public static Coordinates NorthPole() => new(90, 0);
    public static Coordinates SouthPole() => new(-90, 0);
    public static Coordinates Origin() => new(0, 0);
    public Coordinates GetAntipode() => new(-Latitude, -Longitude);
    public bool IsAntipodalTo(Coordinates other) => IsCloseTo(other.GetAntipode());
}
