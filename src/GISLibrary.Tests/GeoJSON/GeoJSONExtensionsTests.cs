using System.Text.Json;
using Tudormobile.GeoJSON;

namespace GISLibrary.Tests.GeoJSON;

/// <summary>
/// Contains unit tests for the <see cref="GeoJSONExtensions"/> class.
/// </summary>
[TestClass]
public class GeoJSONExtensionsTests
{
    /// <summary>
    /// Tests that AsGeometry returns a valid GeoJSONGeometry when the feature has a Point geometry.
    /// </summary>
    [TestMethod]
    public void AsGeometry_WithValidPointGeometry_ReturnsGeoJSONGeometry()
    {
        // Arrange
        var json = """
            {
                "type": "Feature",
                "geometry": {
                    "type": "Point",
                    "coordinates": [102.0, 0.5]
                },
                "properties": {}
            }
            """;
        var featureElement = JsonDocument.Parse(json).RootElement;
        var feature = new GeoJSONFeature(featureElement);

        // Act
        var result = feature.AsGeometry();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Point", result.Type);
    }

    /// <summary>
    /// Tests that AsGeometry returns a valid GeoJSONGeometry when the feature has a LineString geometry.
    /// </summary>
    [TestMethod]
    public void AsGeometry_WithValidLineStringGeometry_ReturnsGeoJSONGeometry()
    {
        // Arrange
        var json = """
            {
                "type": "Feature",
                "geometry": {
                    "type": "LineString",
                    "coordinates": [[102.0, 0.0], [103.0, 1.0], [104.0, 0.0]]
                },
                "properties": {}
            }
            """;
        var featureElement = JsonDocument.Parse(json).RootElement;
        var feature = new GeoJSONFeature(featureElement);

        // Act
        var result = feature.AsGeometry();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("LineString", result.Type);
    }

    /// <summary>
    /// Tests that AsGeometry returns a valid GeoJSONGeometry when the feature has a Polygon geometry.
    /// </summary>
    [TestMethod]
    public void AsGeometry_WithValidPolygonGeometry_ReturnsGeoJSONGeometry()
    {
        // Arrange
        var json = """
            {
                "type": "Feature",
                "geometry": {
                    "type": "Polygon",
                    "coordinates": [
                        [[100.0, 0.0], [101.0, 0.0], [101.0, 1.0], [100.0, 1.0], [100.0, 0.0]]
                    ]
                },
                "properties": {}
            }
            """;
        var featureElement = JsonDocument.Parse(json).RootElement;
        var feature = new GeoJSONFeature(featureElement);

        // Act
        var result = feature.AsGeometry();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("Polygon", result.Type);
    }

    /// <summary>
    /// Tests that AsGeometry returns null when the feature has a null geometry.
    /// </summary>
    [TestMethod]
    public void AsGeometry_WithNullGeometry_ReturnsNull()
    {
        // Arrange
        var json = """
            {
                "type": "Feature",
                "geometry": null,
                "properties": {}
            }
            """;
        var featureElement = JsonDocument.Parse(json).RootElement;
        var feature = new GeoJSONFeature(featureElement);

        // Act
        var result = feature.AsGeometry();

        // Assert
        Assert.IsNull(result);
    }

    /// <summary>
    /// Tests that AsGeometry returns null when the geometry is not an object (e.g., string).
    /// </summary>
    [TestMethod]
    public void AsGeometry_WithNonObjectGeometry_ReturnsNull()
    {
        // Arrange
        var json = """
            {
                "type": "Feature",
                "geometry": "invalid",
                "properties": {}
            }
            """;
        var featureElement = JsonDocument.Parse(json).RootElement;
        var feature = new GeoJSONFeature(featureElement);

        // Act
        var result = feature.AsGeometry();

        // Assert
        Assert.IsNull(result);
    }

    /// <summary>
    /// Tests that AsGeometry returns null when the geometry is an array instead of an object.
    /// </summary>
    [TestMethod]
    public void AsGeometry_WithArrayGeometry_ReturnsNull()
    {
        // Arrange
        var json = """
            {
                "type": "Feature",
                "geometry": [],
                "properties": {}
            }
            """;
        var featureElement = JsonDocument.Parse(json).RootElement;
        var feature = new GeoJSONFeature(featureElement);

        // Act
        var result = feature.AsGeometry();

        // Assert
        Assert.IsNull(result);
    }

    /// <summary>
    /// Tests that AsGeometry returns null when the geometry is a number.
    /// </summary>
    [TestMethod]
    public void AsGeometry_WithNumberGeometry_ReturnsNull()
    {
        // Arrange
        var json = """
            {
                "type": "Feature",
                "geometry": 123,
                "properties": {}
            }
            """;
        var featureElement = JsonDocument.Parse(json).RootElement;
        var feature = new GeoJSONFeature(featureElement);

        // Act
        var result = feature.AsGeometry();

        // Assert
        Assert.IsNull(result);
    }

    /// <summary>
    /// Tests that AsGeometry returns a valid GeoJSONGeometry when the feature has a MultiPoint geometry.
    /// </summary>
    [TestMethod]
    public void AsGeometry_WithValidMultiPointGeometry_ReturnsGeoJSONGeometry()
    {
        // Arrange
        var json = """
            {
                "type": "Feature",
                "geometry": {
                    "type": "MultiPoint",
                    "coordinates": [[100.0, 0.0], [101.0, 1.0]]
                },
                "properties": {}
            }
            """;
        var featureElement = JsonDocument.Parse(json).RootElement;
        var feature = new GeoJSONFeature(featureElement);

        // Act
        var result = feature.AsGeometry();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("MultiPoint", result.Type);
    }

    /// <summary>
    /// Tests that AsGeometry returns a valid GeoJSONGeometry when the feature has a MultiLineString geometry.
    /// </summary>
    [TestMethod]
    public void AsGeometry_WithValidMultiLineStringGeometry_ReturnsGeoJSONGeometry()
    {
        // Arrange
        var json = """
            {
                "type": "Feature",
                "geometry": {
                    "type": "MultiLineString",
                    "coordinates": [
                        [[100.0, 0.0], [101.0, 1.0]],
                        [[102.0, 2.0], [103.0, 3.0]]
                    ]
                },
                "properties": {}
            }
            """;
        var featureElement = JsonDocument.Parse(json).RootElement;
        var feature = new GeoJSONFeature(featureElement);

        // Act
        var result = feature.AsGeometry();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("MultiLineString", result.Type);
    }

    /// <summary>
    /// Tests that AsGeometry returns a valid GeoJSONGeometry when the feature has a MultiPolygon geometry.
    /// </summary>
    [TestMethod]
    public void AsGeometry_WithValidMultiPolygonGeometry_ReturnsGeoJSONGeometry()
    {
        // Arrange
        var json = """
            {
                "type": "Feature",
                "geometry": {
                    "type": "MultiPolygon",
                    "coordinates": [
                        [[[102.0, 2.0], [103.0, 2.0], [103.0, 3.0], [102.0, 3.0], [102.0, 2.0]]],
                        [[[100.0, 0.0], [101.0, 0.0], [101.0, 1.0], [100.0, 1.0], [100.0, 0.0]]]
                    ]
                },
                "properties": {}
            }
            """;
        var featureElement = JsonDocument.Parse(json).RootElement;
        var feature = new GeoJSONFeature(featureElement);

        // Act
        var result = feature.AsGeometry();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("MultiPolygon", result.Type);
    }

    /// <summary>
    /// Tests that AsGeometry returns a valid GeoJSONGeometry when the feature has a GeometryCollection.
    /// </summary>
    [TestMethod]
    public void AsGeometry_WithValidGeometryCollection_ReturnsGeoJSONGeometry()
    {
        // Arrange
        var json = """
            {
                "type": "Feature",
                "geometry": {
                    "type": "GeometryCollection",
                    "geometries": [
                        {
                            "type": "Point",
                            "coordinates": [100.0, 0.0]
                        },
                        {
                            "type": "LineString",
                            "coordinates": [[101.0, 0.0], [102.0, 1.0]]
                        }
                    ]
                },
                "properties": {}
            }
            """;
        var featureElement = JsonDocument.Parse(json).RootElement;
        var feature = new GeoJSONFeature(featureElement);

        // Act
        var result = feature.AsGeometry();

        // Assert
        Assert.IsNotNull(result);
        Assert.AreEqual("GeometryCollection", result.Type);
    }
}
