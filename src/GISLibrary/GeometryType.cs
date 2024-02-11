using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.GISLibrary
{
    /// <summary>
    /// Geometry Types ref: RFC 7946 (1.4)
    /// </summary>
    public enum GeometryType
    {
        /// <summary>
        /// "Unknown" geometry type.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// "Point" geometry type.
        /// </summary>
        Point,

        /// <summary>
        /// "MultiPoint" geometry type.
        /// </summary>
        MultiPoint,

        /// <summary>
        /// "LineString" geometry type.
        /// </summary>
        LineString,

        /// <summary>
        /// "MultiLineString" geometry type.
        /// </summary>
        MultiLineString,

        /// <summary>
        /// "Polygon" geometry type.
        /// </summary>
        Polygon,

        /// <summary>
        /// "MultiPolygon" geometry type.
        /// </summary>
        MultiPolygon,

        /// <summary>
        /// "GeometryCollection" geometry type.
        /// </summary>
        GeometryCollection
    }
}
