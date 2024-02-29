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
    private const double _degToRad = Math.PI / 180;
    private const double _radToDeg = 180.0 / Math.PI;

    public readonly double initialLatitude;
    public readonly double initialLongitude;
    public readonly double initialAzimuth;
    public readonly double nodeAzimuth;
    public readonly double nodeLongitude;
    public readonly double angularDistance_NodeToInitial;

    private readonly double _sinInitLat;
    private readonly double _cosInitLat;
    private readonly double _sinInitLon;
    private readonly double _cosInitLon;
    private readonly double _sinInitAzi;
    private readonly double _cosInitAzi;
    private readonly double _sinNodeAzi;
    private readonly double _cosNodeAzi;
    private readonly double _sinAngDistNodeToInit;
    private readonly double _cosAngDistNodeToInit;
    private readonly double _sinLonDiffNodeToInit;
    private readonly double _cosLonDiffNodeToInit;

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

        // Calculate location of the ascending node
        _sinNodeAzi = _sinInitAzi * _cosInitLat;
        _cosNodeAzi = Math.Sqrt(
            _cosInitAzi * _cosInitAzi
            + _sinInitAzi * _sinInitAzi * _cosInitLat * _cosInitLat);
        nodeAzimuth = Math.Atan2(_sinNodeAzi, _cosNodeAzi) * _radToDeg;

        double tanInitLat = _sinInitLat / _cosInitLat;
        angularDistance_NodeToInitial = Math.Atan2(tanInitLat, _cosInitAzi) * _radToDeg;
        (_sinAngDistNodeToInit, _cosAngDistNodeToInit) = _SinCosFromAtan2Args(
            tanInitLat, _cosInitAzi);
        (_sinLonDiffNodeToInit, _cosLonDiffNodeToInit) = _SinCosFromAtan2Args(
            _sinNodeAzi * _sinAngDistNodeToInit,
            _cosAngDistNodeToInit);
        nodeLongitude = initialLongitude - Math.Atan2(
            _sinNodeAzi * _sinAngDistNodeToInit,
            _cosAngDistNodeToInit) * _radToDeg;
    }

    private static (double, double) _SinCosFromAtan2Args(double y, double x)
    {
        double norm = Math.Sqrt(x * x + y * y);
        return (y / norm, x / norm);
    }

}
