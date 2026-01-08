namespace Tudormobile.Kml;

/// <summary>
/// Defines the contract for a KML item with basic metadata properties.
/// </summary>
public interface IKmlItem
{
    /// <summary>
    /// Gets the unique identifier of the item.
    /// </summary>
    /// <value>A string representing the item's ID.</value>
    string Id { get; }

    /// <summary>
    /// Gets the name of the item.
    /// </summary>
    /// <value>A string representing the item's name.</value>
    string Name { get; }

    /// <summary>
    /// Gets the description of the item.
    /// </summary>
    /// <value>A string representing the item's description.</value>
    string Description { get; }
}


