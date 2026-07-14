namespace Tudormobile.GIS.Fit;

/// <summary>
/// Identifies whether a <see cref="FitRecord"/> is a Definition Message (describes the layout
/// of subsequent Data Messages for a given local message type) or a Data Message (contains
/// raw field bytes laid out according to a previously received Definition Message).
/// </summary>
public enum FitMessageType
{
    /// <summary>
    /// A Data Message containing raw field bytes.
    /// </summary>
    Data,

    /// <summary>
    /// A Definition Message describing the field layout for a local message type.
    /// </summary>
    Definition,
}
