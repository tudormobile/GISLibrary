namespace Tudormobile.GIS;

/// <summary>
/// Defines a mechanism for constructing an instance of type <typeparamref name="T"/>.
/// </summary>
/// <remarks>Implementations of this interface encapsulate the logic required to create objects of type
/// <typeparamref name="T"/>. This pattern is commonly used to separate complex construction processes from the
/// representation of the object itself.</remarks>
/// <typeparam name="T">The type of object to be constructed by the builder.</typeparam>
public interface IBuilder<T>
{
    /// <summary>
    /// Creates and returns an instance of type <typeparamref name="T"/> based on the current configuration.
    /// </summary>
    /// <returns>An instance of type <typeparamref name="T"/> constructed according to the builder's settings.</returns>
    T Build();
}
