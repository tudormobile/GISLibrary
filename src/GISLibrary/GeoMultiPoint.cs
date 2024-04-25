using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.GISLibrary
{
    /// <summary>
    /// 'MultiPoint' geometry object.
    /// </summary>
    public class GeoMultiPoint : GeoPolygon
    {
        /// <inheritdoc/>
        public GeoMultiPoint(Vector2[]? points = null) : base(points)
        {
            GeometryType = GeometryType.MultiPoint;
        }

    }
}
