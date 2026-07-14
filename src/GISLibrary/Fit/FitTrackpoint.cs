namespace Tudormobile.GIS.Fit;

/// <summary>
/// Represents a single decoded FIT "record" (global message 20) sample: a timestamped
/// GPS/sensor observation, typically emitted once per second during an activity.
/// </summary>
/// <param name="Timestamp">The UTC timestamp of the sample, if present in the message.</param>
/// <param name="Position">The decoded position of the sample, if present.</param>
public sealed record FitTrackpoint(DateTime? Timestamp, FitPosition? Position);
