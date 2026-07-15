namespace Tudormobile.GIS.Fit;

/// <summary>
/// Represents a raw geographic position decoded from a FIT "record" message, expressed as
/// independent latitude, longitude, and altitude values (each in decimal degrees / meters).
/// </summary>
/// <remarks>
/// Unlike <see cref="GeoPosition"/>, altitude is optional here: FIT devices may report a
/// position without altitude (or vice versa), so this type keeps the three values decoupled
/// rather than forcing an altitude of zero when one was not actually recorded.
/// </remarks>
/// <param name="Latitude">Latitude, in decimal degrees.</param>
/// <param name="Longitude">Longitude, in decimal degrees.</param>
/// <param name="Altitude">Altitude, in meters, if present in the source message.</param>
public sealed record FitPosition(double Latitude, double Longitude, double? Altitude);
