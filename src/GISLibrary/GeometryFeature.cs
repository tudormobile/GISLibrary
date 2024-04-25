using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tudormobile.GISLibrary
{
    /// <summary>
    /// A geometry with a property map.
    /// </summary>
    public class GeometryFeature
    {
        /// <summary>
        /// The Feature geometry.
        /// </summary>
        public GeometryObject Geometry { get; init; }

        /// <summary>
        /// The feature property map.
        /// </summary>
        public IReadOnlyDictionary<string, string> Properties { get; }

        internal GeometryFeature(IEnumerable<(string name, string value)> properties, GeometryObject geometry)
        {
            Geometry = geometry;
            Properties = properties.ToDictionary(properties => properties.name, properties => properties.value);
        }
    }
}
