using System.Xml.Linq;

namespace Tudormobile.Tcx;

public partial class TcxDocument
{
    /// <summary>
    /// Represents a lap within an activity, containing timing, distance, and trackpoint data.
    /// </summary>
    public class TcxActivityLap : GpxDocumentElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TcxActivityLap"/> class.
        /// </summary>
        /// <param name="activityElement">The XML element representing the lap.</param>
        public TcxActivityLap(XElement activityElement) : base(activityElement) { }

        /// <summary>
        /// Gets the start time of the lap.
        /// </summary>
        public DateTime StartTime => (DateTime)_element.Attribute("StartTime")!;

        /// <summary>
        /// Gets the total elapsed time of the lap in seconds.
        /// </summary>
        public double TotalTimeSeconds => (double)_element.Element(_element.GetDefaultNamespace() + "TotalTimeSeconds")!;

        /// <summary>
        /// Gets the total distance covered in the lap in meters.
        /// </summary>
        public double DistanceMeters => (double)_element.Element(_element.GetDefaultNamespace() + "DistanceMeters")!;

        /// <summary>
        /// Gets the maximum speed achieved during the lap in meters per second.
        /// </summary>
        public double MaximumSpeed => (double)_element.Element(_element.GetDefaultNamespace() + "MaximumSpeed")!;

        /// <summary>
        /// Gets the collection of trackpoints (recorded positions) in the lap.
        /// </summary>
        public IEnumerable<TcxTrackpoint> Tracks => _element.Elements(_element.GetDefaultNamespace() + "Track").Elements(_element.GetDefaultNamespace() + "Trackpoint").Select(x => new TcxTrackpoint(x));
    }
}