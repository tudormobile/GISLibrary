# GeoJSON
This directory contains classes and utilities for working with GeoJSON data in the GISLibrary. 
GeoJSON is a widely used format for encoding geographic data structures using JavaScript Object Notation (JSON).

Ref: [GeoJSON Specification](https://tools.ietf.org/html/rfc7946) RFC7946

> [!NOTE]  
> The capitilization style of GeoJSON (capital JSON) is used to match the convention
> in RFC 7946 specification. All methods involving I/O are implmementd as asynchronous.
### Supported Features
- All Geometry Objects, including GeometryCollection
- Bounding box options
- Properties
- Arbitray objects (members)
- Reading, creating, and writing GeoJSON documents and files

## GeoJSON Documents
- A *GeoJSON document* (DOM) is represented by the `GeoJSONDocument` class.
- A *GeoJSON file* (filesystem) is represented by the `GeoJSONFile` class.

```cs
var file = new GeoJSONFile("path/to/geojsonfile.geojson");
var geoJsonDocument = await file.ReadDocumentAsync();

// or...

using var stream = File.OpenRead("path/to/geojsonfile.geojson");
var geoJsonDocument = await GeoJSONDocument.ParseAsync(stream);

// or ...

var path = "path/to/geojsonfile.geojson";
var geoJsonDocument = await GeoJSONDocument.LoadFromFileAsync(path);
// ...
```

## GeoJSON Primitive Types
The following GeoJSON primitive types are implemented:
- `GeoJSONPosition`: Represents a geographic position (longitude, latitude, and optional altitude).
- `GeoJSONBBox`: Represents a bounding box for a geometry.  

## GeoJSON Geometry Types
The seven GeoJSON Gemoetry types:
- `GeoJSONPoint`: Represents a single geographic point.
- `GeoJSONLineString`: Represents a series of connected geographic points (a line).
- `GeoJSONPolygon`: Represents a polygon defined by a series of geographic points.
- `GeoJSONMultiPoint`: Represents multiple geographic points.
- `GeoJSONMultiLineString`: Represents multiple line strings.
- `GeoJSONMultiPolygon`: Represents multiple polygons.
- `GeoJSONGeometryCollection`: Represents a collection of geometry objects.

## GeoJSON Complex Types
Complex types:
- `GeoJSONFeature`: Represents a single feature with geometry and properties.
- `GeoJSONFeatureCollection`: Represents a collection of features.

## Low-level (base) types
The library utilizes some base types to facilitate constructing and exporting GeoJSON geometries to and from Json notations:
- `GeoJSONCoordinates`: Represents a set of coordinates for various GeoJSON geometries. Most geometry types derive from this *abstract record* class.

## GeoJSON Builders
The `IGeoJSONDocumentBuilder` interface provides a fluent interface for constructing GeoJSON documents programmatically. 
Use the ***Create()*** static method on the `GeoJSONDocument` class to obtain an instance of the builder. Methods
are provided to add properties, arbitrary data (objects), and various GeoJSON geometries (*features*) to the document. The GeoJSON geometries can also include properties, bounding-box values, and arbitrary objects in addition to their types and coordinates values.  
- `IGeoJSONDocumentBuilder`: Adds features to a document. Obtain via ***GeoJSONDocument.Create()***. Also adds properties and objects.
- `IGeoJSONFeatureBuilder`: Creates features from Geometries. Implicitly created via the ***AddFeature()*** method on the document builder for a given geometry. Also adds properties and objects.

Example:
```cs
var point = new GeoJSONPoint(1.0, 2.0, 3.0);

var document = GeoJSONDocument.Create()
      .AddProperty("name", "Test GeoJSON Document")
      .AddFeature(f => f
          .SetGeometry(point)
          .AddObject("id", 1)
          .AddProperty("description", "This is point 1"))
      .AddFeature(f => f
          .SetGeometry(new GeoJSONLineString(
            [
              new GeoJSONPosition(1.0, 2.0),
              new GeoJSONPosition(3.0, 4.0)
            ]
            )
          .AddObject("id", 3)
          .AddProperty("description", "This is a linestring"))
      .Build();

await document.SaveToFileAsync("output.geojson");
// or...
await document.SaveToAsync(stream);
// or...
await document.WriteToAsync(utf8JsonWriter);
```
The Save/Write overloads allow flexibility and extension opportunities.
## GeoJSON Geometries
### Creating Geometries
The GeoJSON geometries are created from GeoJSON files using the Document Object Model (DOM) of the GeoJSONDocument. The ***FeatureCollection*** property holds a collection of *features*, each containing a ***Geometry*** property (for the *geometry*), ***BoundingBox*** property (optional *bounding box*) and a ***Properties*** property (to access the *properties*). The GeoJSON geomteries each have a ***Coordinates*** property to access the specific geometry records.
#### Geometry Records
Geometry records are created when reading/parsing GeoJSON files and they may also be created manually to construct GeoJSON documents and files. There are records to represent all seven GeoJSON geometries, and they are built from the basic building block - which is the ***Position*** structure.
```cs
struct GeoJSONPosition
{
    double Longitude { get; set; }
    double Latitude { get; set; }
    double? Altitude { get; set; }
}
```
Points may be implicitly converted to/from positions. Altitude is optional.
- `GeoJSONPoint`: A single position.
- `GeoJSONMultiPoint`: A collection of points (or positions).
- `GeoJSONLineString`: A collection of positions.
- `GeoJSONMultiLineString`: A collection of lines.
- `GeoJSONPolygon`: A collection of 'Rings' (which are collections of lines)
- `GeoJSONMultiPolygon`: A collection of polygons.
- `GeoJSONGeometryCollection`: A collection of geometries.
Each of the geometries can export their values as *coordinates* in the appropriate Json format.
> [!IMPORTANT]  
> Many geographical/mapping systems express coordinates as (latitude,longitude) pairs.
> This convention was adopted when creating positions, however, the GeoJSON format
> expresses coordinates in [*longitude*, *latitude* (, *altitude*)] order. Keep
> this in mind when creating geometries.

