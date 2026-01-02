using System.Xml.Linq;

namespace Tudormobile.Gpx;

public partial class GpxDocument
{
    /// <summary>
    /// Represents a route in a GPX document.
    /// </summary>
    public class GpxRoute : GpxEntity
    {
        internal GpxRoute(XElement element) : base(element) { }

        /// <summary>
        /// Gets the route number.
        /// </summary>
        public long Number => long.Parse(_element.Element(_element.GetDefaultNamespace() + "number")?.Value ?? "0");

        /// <summary>
        /// Gets the list of route points in the route.
        /// </summary>
        public List<GpxWaypoint> RoutePoints => [.. _element.Elements(_element.GetDefaultNamespace() + "rtept").Select(x => new GpxWaypoint(x))];
    }
}
