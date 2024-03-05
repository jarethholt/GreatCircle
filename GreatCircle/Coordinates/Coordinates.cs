// Class describing a coordinate (latitude/longitude) on the sphere.

using GreatCircle.MyMathUtils;

namespace GreatCircle.Coordinates;

public readonly struct Coordinate
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
    public const string degreeSymbol = "\u00B0";
    private readonly double _latitude;
    private readonly double _longitude;

    public Coordinate(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public double Latitude
    {
        readonly get => _latitude;
        init
        {
            // Check that latitude is between -90 and 90
            if (Math.Abs(value) > 90)
                throw new ArgumentOutOfRangeException(
                    nameof(value),
                    "The latitude must be between -90 and 90");

            _latitude = value;
        }
    }

    public double Longitude
    {
        readonly get => _longitude;
        init
        {
            // Place the value in the range [0, 360)
            value %= 360;
            // Place the longitude in the range [-180, 180)
            _longitude = value < 180 ? value : value - 360;
        }
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
            degreeSymbol,
            northOrSouth,
            eastOrWest);
        return string.Format(messageFormat, Math.Abs(Latitude), Math.Abs(Longitude));
    }

    // Determine whether the coordinate should be considered a polar point.
    public bool IsAPole => MyMath.IsCloseTo(Math.Abs(Latitude), 90);

    public bool IsCloseTo(Coordinate other)
    {
        // Test if two coordinates are too close to consider distinct
        // Polar coordinates can be given with any longitude
        bool result = MyMath.AreClose(Latitude, other.Latitude);
        if (this.IsAPole || other.IsAPole)
            return result;
        result &= MyMath.AreClose(Longitude, other.Longitude);
        return result;
    }

    public static Coordinate NorthPole => new(90, 0);
    public static Coordinate SouthPole => new(-90, 0);
    public static Coordinate Origin => new(0, 0);
    public Coordinate Antipode => new(-Latitude, Longitude + 180);
    public bool IsAntipodalTo(Coordinate other) => IsCloseTo(other.Antipode);
}
