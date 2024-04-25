using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Tudormobile.GISLibrary
{
    /// <summary>
    /// Builds geometry objects.
    /// </summary>
    public class GeometryBuilder
    {

        /// <summary>
        /// Create a GeometryFeature from json.
        /// </summary>
        /// <param name="json">json string to parse.</param>
        /// <returns>Geometry feature.</returns>
        public static IEnumerable<GeometryFeature> FeaturesFromJson(string json)
        {
            var jo = JsonObject.Parse(json);
            if (jo != null)
            {
                foreach (var feature in jo["features"].AsArray())
                {
                    yield return new GeometryFeature(propertyMapFromNode(feature["properties"]), geoObjectFromNode(feature["geometry"]));

                }
            }
        }


        private static IEnumerable<(string name, string value)> propertyMapFromNode(JsonNode? node)
            => node?.AsObject().Select(o => (o.Key, o.Value?.ToString() ?? String.Empty)) ?? Enumerable.Empty<(string name, string value)>();

        /// <summary>
        /// Create Geometry Object from json string.
        /// </summary>
        /// <param name="json">Json string.</param>
        /// <returns>Geometry object.</returns>
        /// <exception cref="NotImplementedException"></exception>
        public static GeometryObject FromJson(string json)
            => geoObjectFromNode(JsonObject.Parse(json));

        private static GeometryObject geoObjectFromNode(JsonNode? jo)
        {
            if (jo != null && Enum.TryParse<GeometryType>(jo["type"]?.GetValue<String?>() ?? "Unknown", out var geoType))
            {
                return geoType switch
                {
                    GeometryType.Point => throw new NotImplementedException(),
                    GeometryType.MultiPoint => throw new NotImplementedException(),
                    GeometryType.Polygon => geoPolygonFromNode(jo),
                    GeometryType.MultiPolygon => geoMultiPolygonFromNode(jo),
                    GeometryType.LineString => throw new NotImplementedException(),
                    GeometryType.MultiLineString => throw new NotImplementedException(),
                    GeometryType.GeometryCollection => throw new NotImplementedException(),
                    GeometryType.Unknown => GeometryObject.Unknown,
                    _ => GeometryObject.Unknown
                };
            }
            return GeometryObject.Unknown;
        }

        private static GeoPolygon geoPolygonFromNode(JsonNode node)
            => new(vectorsFromNode(node["coordinates"]).ToArray());
        private static GeoMultiPolygon geoMultiPolygonFromNode(JsonNode jo)
            => new(vectorArraysFromNode(jo["coordinates"]).ToArray());
        private static IEnumerable<Vector2[]> vectorArraysFromNode(JsonNode? node)
            => node?.AsArray()?.Select(n => vectorsFromNode(n).ToArray()) ?? Enumerable.Empty<Vector2[]>();
        private static IEnumerable<Vector2> vectorsFromNode(JsonNode? node)
            => node?[0]?.AsArray()?.Select(n => vectorFromNode(n)) ?? Enumerable.Empty<Vector2>();
        private static Vector2 vectorFromNode(JsonNode? node)
            => new(node?[0]?.GetValue<Single>() ?? 0, node?[1]?.GetValue<Single>() ?? 0);
    }
}
