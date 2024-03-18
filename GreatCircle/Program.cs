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
            $"Central angle between these points: {angle:F2}{Angles.Degree}"
        );
        Console.WriteLine(
            $"Required initial azimuth: {path.InitialAzimuth:F2}{Angles.Degree}"
        );
    }
}
