namespace Tudormobile.GIS.Fit;

/// <summary>
/// Describes a single developer-defined field within a FIT Definition Message.
/// </summary>
/// <param name="FieldDefinitionNumber">The developer field number.</param>
/// <param name="Size">The size, in bytes, of the field's encoded value.</param>
/// <param name="DeveloperDataIndex">The index identifying the developer data ID this field belongs to.</param>
public sealed record FitDeveloperFieldDefinition(byte FieldDefinitionNumber, byte Size, byte DeveloperDataIndex);
