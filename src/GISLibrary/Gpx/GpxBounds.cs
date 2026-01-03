using System.Xml.Linq;

namespace Tudormobile.Gpx;

public partial class GpxDocument
{
    /// <summary>
    /// Represents the geographic bounds of a GPX document.
    /// </summary>
    public class GpxBounds
    {
        /// <summary>
        /// Gets or sets the minimum latitude.
        /// </summary>
        public double MinLat { get; set; }

        /// <summary>
        /// Gets or sets the minimum longitude.
        /// </summary>
        public double MinLon { get; set; }

        /// <summary>
        /// Gets or sets the maximum latitude.
        /// </summary>
        public double MaxLat { get; set; }

        /// <summary>
        /// Gets or sets the maximum longitude.
        /// </summary>
        public double MaxLon { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GpxBounds"/> class.
        /// </summary>
        /// <param name="boundsElement">The XML element containing bounds data.</param>
        public GpxBounds(XElement? boundsElement)
        {
            if (boundsElement != null)
            {
                MinLat = double.Parse(boundsElement.Attribute("minlat")?.Value ?? "0");
                MinLon = double.Parse(boundsElement.Attribute("minlon")?.Value ?? "0");
                MaxLat = double.Parse(boundsElement.Attribute("maxlat")?.Value ?? "0");
                MaxLon = double.Parse(boundsElement.Attribute("maxlon")?.Value ?? "0");
            }
        }

        /// <summary>
        /// Returns a string representation of the bounds.
        /// </summary>
        /// <returns>A formatted string containing the minimum and maximum latitude and longitude values.</returns>
        public override string ToString()
        {
            return $"(MinLat: {MinLat}, MinLon: {MinLon}, MaxLat: {MaxLat}, MaxLon: {MaxLon})";
        }
    }
}
