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
                throw new ArgumentOutOfRangeException(nameof(value), "The latitude must be between -90 and 90");
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
}
