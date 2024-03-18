namespace GreatCircle;

internal class Program
{
    static void Main()
    {
        Coordinate valparaiso = new(-33.0, -71.6);
        Console.WriteLine($"Coordinates of Valparaiso: {valparaiso.ToString("F1")}");
        Coordinate shanghai = new(31.4, 121.8);
        Console.WriteLine($"Coordinates of Shanghai: {shanghai.ToString("F1")}");

        (GreatCirclePath path, double angle)
            = GreatCirclePath.PathAndAngleBetweenPoints(valparaiso, shanghai);

        Console.WriteLine(
            $"Central angle between these points (should be 168.56{Angles.Degree}): "
            + $"{angle:F2}{Angles.Degree}"
        );
        Console.WriteLine(
            $"Required initial azimuth (should be -94.41{Angles.Degree}): "
            + $"{path.InitialAzimuth:F2}{Angles.Degree}"
        );

        (Coordinate target, double finalAzimuth) = path.DisplaceByAngle(angle);
        Console.WriteLine(
            $"Displacing by {angle:F2}{Angles.Degree} from {valparaiso.ToString("F1")} "
            + $"along heading {path.InitialAzimuth:F2}{Angles.Degree} gets us to "
            + $"{target.ToString("F1")} (should be {shanghai.ToString("F1")})."
        );
        Console.WriteLine(
            $"We also get a final azimuth (should be -78.42{Angles.Degree}): "
            + $"{finalAzimuth:F2}");
    }
}
