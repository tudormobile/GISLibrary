namespace Tudormobile.GIS.Fit;

/// <summary>
/// Describes a single field within a FIT Definition Message.
/// </summary>
/// <param name="FieldDefinitionNumber">The field number, as defined by the FIT global profile for the message.</param>
/// <param name="Size">The size, in bytes, of the field's encoded value.</param>
/// <param name="BaseType">The raw FIT base type identifier for the field.</param>
public sealed record FitFieldDefinition(byte FieldDefinitionNumber, byte Size, byte BaseType);
