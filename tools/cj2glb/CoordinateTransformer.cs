namespace cj2glb;

using DotSpatial.Projections;
using NetTopologySuite.Geometries;
using System;

public static class CoordinateTransformer
{
    public static Coordinate TransformToWGS84(double x, double y, double altitude, string sourceCrsUrn)
    {
        var epsgCode = ExtractEpsgCode(sourceCrsUrn);
        if (epsgCode != null)
        {
            double[] xy = [ x, y ];
            double[] z = [altitude];

            ProjectionInfo fromProjection = ProjectionInfo.FromEpsgCode(epsgCode.Value);
            ProjectionInfo toWGS84 = KnownCoordinateSystems.Geographic.World.WGS1984;

            Reproject.ReprojectPoints(xy, z, fromProjection, toWGS84, 0, 1);
            return new Coordinate(xy[0], xy[1]);
        }

        return null;
    }

    private static int? ExtractEpsgCode(string urn)
    {
        if (string.IsNullOrWhiteSpace(urn))
            throw new ArgumentException("URN is null or empty.");

        var parts = urn.Split(':', '/');
        
        for (int i = 0; i < parts.Length; i++)
        {
            if (parts[i].Equals("EPSG", StringComparison.OrdinalIgnoreCase) && i + 2 < parts.Length)
            {
                if (int.TryParse(parts[i + 2], out int epsg))
                    return epsg;
            }
        }

        if (parts.Length >= 1 && int.TryParse(parts[^1], out int epsgFallback))
            return epsgFallback;

        throw new FormatException("URN does not contain a valid EPSG code.");
    }
}