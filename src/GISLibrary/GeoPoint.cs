using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.GISLibrary
{
    /// <summary>
    /// 'Point' gemoetry object
    /// </summary>
    public class GeoPoint : GeometryObject
    {
        /// <summary>
        /// 'X' coordinate of the point.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        /// 'Y' coordinate of the point.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        /// Create and initialize a new Geometry Object.
        /// </summary>
        /// <param name="x">'X' coordinate (optional)</param>
        /// <param name="y">'Y' coordinate (optional)</param>
        /// <remarks>
        /// Providing coordinates at construction is optional; the
        /// default value for coordinates is zero.
        /// </remarks>
        public GeoPoint(double x = 0, double y = 0)
        {
            GeometryType = GeometryType.Point;
            X = x;
            Y = y;
        }
    }
}
