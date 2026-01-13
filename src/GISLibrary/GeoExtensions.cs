using Tudormobile.GeoJSON;
using Tudormobile.Gpx;
using Tudormobile.Kml;
using Tudormobile.Tcx;

namespace Tudormobile.GIS;

/// <summary>
/// Provides extension methods for working with geographic features and geometries in GeoJSON format.
/// </summary>
public static class GeoExtensions
{
    /// <summary>
    /// Converts a KML placemark to a GeoPath by extracting coordinates from its geometry.
    /// </summary>
    /// <param name="placemark">The KML placemark to convert. Cannot be null.</param>
    /// <returns>
    /// A GeoPath containing the coordinates from the placemark's geometry, or null if the geometry is not a line or polygon.
    /// </returns>
    public static GeoPath? AsGeoPath(this KmlPlacemark placemark)
    => placemark.Geometry switch
    {
        KmlLineString lineString => new GeoPath().AddRange(lineString.Coordinates.Select(coord => new GeoPosition(coord.Latitude, coord.Longitude, coord.Altitude))),
        KmlPolygon polygon => new GeoPath().AddRange(polygon.OuterBoundary.Select(coord => new GeoPosition(coord.Latitude, coord.Longitude, coord.Altitude))),
        _ => null
    };

    /// <summary>
    /// Converts the first ring of the specified GeoJSON polygon to a GeoPath instance.
    /// </summary>
    /// <remarks>Only the first ring of the GeoJSON polygon is converted. Any additional rings, such as holes,
    /// are ignored.</remarks>
    /// <param name="geoJsonPolygon">The GeoJSON polygon whose first ring will be converted. Cannot be null.</param>
    /// <returns>A GeoPath representing the first ring of the polygon, or null if the polygon has no rings.</returns>
    public static GeoPath? AsGeoPath(this GeoJSONPolygon geoJsonPolygon)
    {
        if (geoJsonPolygon.Rings == null || geoJsonPolygon.Rings.Count == 0)
        {
            return null;
        }
        return new GeoPath()
            .AddRange(geoJsonPolygon.Rings.First().Positions.Select(p => new GeoPosition(p.Latitude, p.Longitude, p.Altitude ?? 0)));
    }

    /// <summary>
    /// Converts a TCX activity to a GeoPath by extracting all trackpoints from all laps.
    /// </summary>
    /// <param name="tcxActivity">The TCX activity to convert. Cannot be null.</param>
    /// <returns>A GeoPath containing all trackpoints from the activity's laps, or null if no trackpoints are found.</returns>
    public static GeoPath? AsGeoPath(this TcxDocument.TcxActivity tcxActivity)
        => tcxActivity.Laps.SelectMany(lap => lap.Tracks).AsGeoPath();

    /// <summary>
    /// Converts a collection of TCX trackpoints to a GeoPath.
    /// </summary>
    /// <param name="tcxTrackpoints">The collection of TCX trackpoints to convert. Cannot be null.</param>
    /// <returns>A GeoPath containing the positions from all trackpoints, or null if the collection is empty.</returns>
    public static GeoPath? AsGeoPath(this IEnumerable<TcxDocument.TcxTrackpoint> tcxTrackpoints)
        => new GeoPath().AddRange(tcxTrackpoints.Select(trackpoint => new GeoPosition(trackpoint.Position.lat, trackpoint.Position.lon, trackpoint.AltitudeMeters)));

    /// <summary>
    /// Converts a collection of GPX waypoints to a GeoPath.
    /// </summary>
    /// <param name="gpxWaypoints">The collection of GPX waypoints to convert. Cannot be null.</param>
    /// <returns>A GeoPath containing the positions from all waypoints, or null if the collection is empty.</returns>
    public static GeoPath? AsGeoPath(this IEnumerable<GpxDocument.GpxWaypoint> gpxWaypoints)
        => new GeoPath().AddRange(gpxWaypoints.Select(wayPoint => new GeoPosition(wayPoint.Latitude, wayPoint.Longitude, wayPoint.Elevation)));

    /// <summary>
    /// Converts a collection of GPX track segments to a GeoPath by extracting all track points from all segments.
    /// </summary>
    /// <param name="gpxTrackSegments">The collection of GPX track segments to convert. Cannot be null.</param>
    /// <returns>A GeoPath containing all track points from all segments, or null if no track points are found.</returns>
    public static GeoPath? AsGeoPath(this IEnumerable<GpxDocument.GpxTrackSegment> gpxTrackSegments)
        => gpxTrackSegments.SelectMany(segment => segment.TrackPoints).AsGeoPath();

    /// <summary>
    /// Converts a GPX track to a GeoPath by extracting all track points from all track segments.
    /// </summary>
    /// <param name="gpxTrack">The GPX track to convert. Cannot be null.</param>
    /// <returns>A GeoPath containing all track points from the track's segments, or null if no track points are found.</returns>
    public static GeoPath? AsGeoPath(this GpxDocument.GpxTrack gpxTrack)
        => gpxTrack.TrackSegments.AsGeoPath();

    /// <summary>
    /// Determines whether the specified geographic position lies within the given geographic path.
    /// </summary>
    /// <param name="geoPath">The geographic path to check for containment of the position. Cannot be null.</param>
    /// <param name="position">The geographic position to test for inclusion within the path.</param>
    /// <returns>true if the specified position is within the geographic path; otherwise, false.</returns>
    public static bool IsLocationInPath(this GeoPath geoPath, GeoPosition position)
        => geoPath.IsPositionInPath(new GeoLocation(position.Latitude, position.Longitude));

    /// <summary>
    /// Determines whether the specified geographic location is inside the given path polygon.
    /// </summary>
    /// <remarks>The method uses the ray-casting algorithm to determine point-in-polygon inclusion. The
    /// polygon is assumed to be closed; if the path is not explicitly closed, the first and last points are implicitly
    /// connected. The result is undefined if the path contains fewer than three points.</remarks>
    /// <param name="geoPath">The geographic path that defines the polygon to test. Must contain at least three points to form a valid
    /// polygon.</param>
    /// <param name="location">The geographic location to test for inclusion within the polygon defined by the path.</param>
    /// <returns>true if the specified location is inside the polygon; otherwise, false.</returns>
    public static bool IsPositionInPath(this GeoPath geoPath, GeoLocation location)
    {
        // Ray-casting algorithm to determine if point is in polygon
        bool inside = false;
        int n = geoPath.Count;
        for (int i = 0, j = n - 1; i < n; j = i++)
        {
            if (((geoPath[i].Latitude > location.Latitude) != (geoPath[j].Latitude > location.Latitude)) &&
                (location.Longitude < (geoPath[j].Longitude - geoPath[i].Longitude) * (location.Latitude - geoPath[i].Latitude) / (geoPath[j].Latitude - geoPath[i].Latitude) + geoPath[i].Longitude))
            {
                inside = !inside;
            }
        }
        return inside;
    }
}
