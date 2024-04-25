using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.GISLibrary
{
    /// <summary>
    /// A geometry objects.
    /// </summary>
    public class GeometryObject
    {
        /// <summary>
        /// 'Unknown' Geometry objects
        /// </summary>
        public static readonly GeometryObject Unknown = new() { GeometryType = GeometryType.Unknown };

        /// <summary>
        /// Type of geometry.
        /// </summary>
        public GeometryType GeometryType { get; init; }
    }
}
