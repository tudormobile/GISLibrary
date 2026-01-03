using System.Xml.Linq;

namespace Tudormobile.Gpx;

public partial class GpxDocument
{
    /// <summary>
    /// Represents a waypoint in a GPX document.
    /// </summary>
    public class GpxWaypoint : GpxEntity
    {
        internal GpxWaypoint(XElement element) : base(element) { }

        /// <summary>
        /// Gets the latitude of the waypoint in decimal degrees.
        /// </summary>
        public double Latitude => double.Parse(_element.Attribute("lat")?.Value ?? "0");

        /// <summary>
        /// Gets the longitude of the waypoint in decimal degrees.
        /// </summary>
        public double Longitude => double.Parse(_element.Attribute("lon")?.Value ?? "0");

        /// <summary>
        /// Gets the elevation of the waypoint in meters.
        /// </summary>
        public double Elevation => double.Parse(_element.Element(_element.GetDefaultNamespace() + "ele")?.Value ?? "0");

        /// <summary>
        /// Gets the timestamp of when the waypoint was recorded.
        /// </summary>
        public DateTime Time
        {
            get
            {
                var timeString = _element.Element(_element.GetDefaultNamespace() + "time")?.Value;
                if (timeString != null && DateTime.TryParse(timeString, out DateTime result))
                {
                    return result;
                }
                return DateTime.MinValue;
            }
        }
    }
}
