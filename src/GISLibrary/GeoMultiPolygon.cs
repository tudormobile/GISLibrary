using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.GISLibrary
{
    /// <summary>
    /// 'Polygon' geometry object.
    /// </summary>
    public class GeoMultiPolygon : GeometryObject
    {
        /// <summary>
        /// Polygons.
        /// </summary>
        public Vector2[][] Polygons { get; set; }

        /// <summary>
        /// Create and initialize a new Geometry Object.
        /// </summary>
        /// <param name="polygons">Polygons that make up the multi-polygon (optional)</param>
        /// <remarks>
        /// Providing polygons at construction is optional.
        /// </remarks>
        public GeoMultiPolygon(Vector2[][]? polygons = null)
        {
            GeometryType = GeometryType.MultiPolygon;
            Polygons = polygons ?? [];
        }

    }
}

