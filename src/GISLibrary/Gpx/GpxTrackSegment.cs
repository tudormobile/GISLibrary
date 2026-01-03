using System.Xml.Linq;

namespace Tudormobile.Gpx;

public partial class GpxDocument
{
    /// <summary>
    /// Represents a track segment within a track in a GPX document.
    /// </summary>
    public class GpxTrackSegment : GpxDocumentElement
    {
        internal GpxTrackSegment(XElement element) : base(element) { }

        /// <summary>
        /// Gets the list of track points in the track segment.
        /// </summary>
        public List<GpxWaypoint> TrackPoints => [.. _element.Elements(_element.GetDefaultNamespace() + "trkpt").Select(x => new GpxWaypoint(x))];
    }
}
