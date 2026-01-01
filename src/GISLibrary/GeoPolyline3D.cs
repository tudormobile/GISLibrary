namespace Tudormobile.GIS;

using System.Collections.Generic;

/// <summary>
/// Represents a three-dimensional polyline defined by a sequence of geographic points.
/// </summary>
/// <remarks>A GeoPolyline3D models a connected series of points in three-dimensional geographic space, such as a
/// flight path or terrain profile. The structure inherits from List&lt;GeoPoint3D&gt;, allowing direct access to standard
/// list operations for managing the collection of points.
/// </remarks>
public class GeoPolyline3D : List<GeoPoint3D>
{
    /// <summary>
    /// Gets the collection of 3D geographic points that define the geometry.
    /// </summary>
    public List<GeoPoint3D> Points => this;
}
