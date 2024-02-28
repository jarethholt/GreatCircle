using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreatCircle;

internal class GreatCirclePath
{
    /* Representation of a generic (non-polar) great circle path.
     * 
     * A great circle path given by an initial point and an azimuth.
     * Points along the path are then parameterized by a given central
     * angle from the initial point.
     * 
     * Fields
     * ------
     * initialLatitude: Latitude of the initial point in degrees N.
     *   Must be between -90 and 90.
     *   NB: At the moment pure meridional (polar) paths are not implemented,
     *   so this field is constrained to (-90, 90).
     * initialLongitude: Longitude of the initial point in degrees E.
     *   Can take any value but will be normalized to the range [-180, 180)
     * initialAzimuth: Azimuth of the path at the initial point
     *   in degrees clockwise from northward, also normalized to [-180, 180).
     *   NB: At the moment purely meridional paths are not implemented, so
     *   this field is constrained to (-180, 0) and (0, 180).
     */
    public readonly double initialLatitude;
    public readonly double initialLongitude;
    public readonly double initialAzimuth;
    private readonly double _sinInitLat;
    private readonly double _cosInitLat;
    private readonly double _sinInitLon;
    private readonly double _cosInitLon;
    private readonly double _sinInitAzi;
    private readonly double _cosInitAzi;
    private const double _degToRad = Math.PI / 180;

    public GreatCirclePath(
        double initialLatitude,
        double initialLongitude,
        double initialAzimuth)
    {
        // Check and add primary properties
        if (Math.Abs(initialLatitude) > 90)
            throw new ArgumentOutOfRangeException(
                nameof(initialLatitude),
                "Latitude must be between -90 and 90");
        if (MyMathUtils.MyMath.IsCloseTo(Math.Abs(initialLatitude), 90))
            throw new NotImplementedException(
                "Paths passing through the poles have not been implemented yet");
        this.initialLatitude = initialLatitude;

        initialLongitude %= 360;
        initialLongitude = initialLongitude < 180 ? initialLongitude : initialLongitude - 360;
        this.initialLongitude = initialLongitude;

        initialAzimuth %= 360;
        initialAzimuth = initialAzimuth < 180 ? initialAzimuth : initialAzimuth - 360;
        if (
            MyMathUtils.MyMath.IsCloseTo(initialAzimuth, 0)
            || MyMathUtils.MyMath.IsCloseTo(initialAzimuth, -180)
            || MyMathUtils.MyMath.IsCloseTo(initialAzimuth, 180)
        )
            throw new NotImplementedException(
                "Purely meridional paths have not been implemented yet");
        this.initialAzimuth = initialAzimuth;

        // Add the other useful fields
        (_sinInitLat, _cosInitLat) = Math.SinCos(this.initialLatitude * _degToRad);
        (_sinInitLon, _cosInitLon) = Math.SinCos(this.initialLongitude * _degToRad);
        (_sinInitAzi, _cosInitAzi) = Math.SinCos(this.initialAzimuth * _degToRad);
    }

}
