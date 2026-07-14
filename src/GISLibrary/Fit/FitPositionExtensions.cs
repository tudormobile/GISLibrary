namespace Tudormobile.GIS.Fit;

/// <summary>
/// Conversion helpers for translating <see cref="FitPosition"/> values into the library's
/// core geographic types (<see cref="GeoPosition"/>, <see cref="GeoLocation"/>).
/// </summary>
public static class FitPositionExtensions
{
    /// <summary>
    /// Converts this <see cref="FitPosition"/> to a <see cref="GeoPosition"/>, substituting
    /// <paramref name="defaultAltitude"/> when no altitude was recorded.
    /// </summary>
    /// <param name="position">The FIT position to convert.</param>
    /// <param name="defaultAltitude">The altitude to use when <see cref="FitPosition.Altitude"/> is <c>null</c>.</param>
    public static GeoPosition ToGeoPosition(this FitPosition position, double defaultAltitude = 0) =>
        new(position.Latitude, position.Longitude, position.Altitude ?? defaultAltitude);

    /// <summary>
    /// Converts this <see cref="FitPosition"/> to a <see cref="GeoLocation"/>, discarding altitude.
    /// </summary>
    /// <param name="position">The FIT position to convert.</param>
    public static GeoLocation ToGeoLocation(this FitPosition position) =>
        new(position.Latitude, position.Longitude);
}
