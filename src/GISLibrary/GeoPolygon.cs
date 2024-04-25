using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.GISLibrary
{
    /// <summary>
    /// 'Polygon' geometry object.
    /// </summary>
    public class GeoPolygon : GeometryObject
    {
        /// <summary>
        /// Polygon points.
        /// </summary>
        public Vector2[] Points { get; set; }

        /// <summary>
        /// Create and initialize a new Geometry Object.
        /// </summary>
        /// <param name="points">Points that make up the polygon (optional)</param>
        /// <remarks>
        /// Providing points at construction is optional.
        /// </remarks>
        public GeoPolygon(Vector2[]? points = null)
        {
            GeometryType = GeometryType.Polygon;
            Points = points ?? [];
        }

    }
}
