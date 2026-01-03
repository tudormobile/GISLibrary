using System.Xml.Linq;

namespace Tudormobile.Gpx;

public partial class GpxDocument
{
    /// <summary>
    /// Represents a track in a GPX document.
    /// </summary>
    public class GpxTrack : GpxEntity
    {
        internal GpxTrack(XElement element) : base(element) { }

        /// <summary>
        /// Gets the track number.
        /// </summary>
        public long Number => long.Parse(_element.Element(_element.GetDefaultNamespace() + "number")?.Value ?? "0");

        /// <summary>
        /// Gets the list of track segments in the track.
        /// </summary>
        public List<GpxTrackSegment> TrackSegments => [.. _element.Elements(_element.GetDefaultNamespace() + "trkseg").Select(x => new GpxTrackSegment(x))];
    }
}
