# GeoJSON
This directory contains classes and utilities for working with GeoJSON data in the GISLibrary. 
GeoJSON is a widely used format for encoding geographic data structures using JavaScript Object Notation (JSON).

Ref: [GeoJSON Specification](https://tools.ietf.org/html/rfc7946)

> [!NOTE]  
> The capitilization style of GeoJSON (capital JSON) is used to match the convention
> in RFC 7946 specification. All methods involving I/O are implmementd as asynchronous.

## GeoJSON Documents
- A *GeoJSON document* (DOM) is represented by the `GeoJSONDocument` class.
- A *GeoJSON file* (filesystem) is represented by the `GeoJSONFile` class.

## GeoJSON Primitive Types
The following GeoJSON primitive types are implemented:
- `GeoJSONPoint`: Represents a single geographic point.
- `GeoJSONLineString`: Represents a series of connected geographic points (a line).
- `GeoJSONPolygon`: Represents a polygon defined by a series of geographic points.
- `GeoJSONMultiPoint`: Represents multiple geographic points.
- `GeoJSONMultiLineString`: Represents multiple line strings.
- `GeoJSONMultiPolygon`: Represents multiple polygons.
- `GeoJSONGeometryCollection`: Represents a collection of geometry objects.
- `GeoJSONFeature`: Represents a single feature with geometry and properties.
- `GeoJSONFeatureCollection`: Represents a collection of features.
- `GeoJSONBoundingBox`: Represents a bounding box defined by geographic coordinates.
- `GeoJSONCRS`: Represents the Coordinate Reference System used in the GeoJSON data.
- `GeoJSONProperties`: Represents the properties associated with a GeoJSON feature.
- `GeoJSONPosition`: Represents a geographic position (longitude, latitude, and optional altitude).
- `GeoJSONCoordinates`: Represents a set of coordinates for various GeoJSON geometries.
- `GeoJSONUtilities`: Provides utility functions for working with GeoJSON data.
- 
- `GeoJSONParser`: Provides functionality to parse GeoJSON data from strings or files.
- `GeoJSONSerializer`: Provides functionality to serialize GeoJSON objects to strings or files.
- `GeoJSONValidator`: Provides functionality to validate GeoJSON data against the GeoJSON specification.
- `GeoJSONConverter`: Provides functionality to convert between GeoJSON and other geographic data formats.
- `GeoJSONRenderer`: Provides functionality to render GeoJSON data on maps or visualizations.
- `GeoJSONStyler`: Provides functionality to style GeoJSON features for rendering.
- `GeoJSONTransformer`: Provides functionality to transform GeoJSON geometries (e.g., scaling, rotating).
- 