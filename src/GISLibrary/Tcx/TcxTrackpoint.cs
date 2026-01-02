using System.Xml.Linq;

namespace Tudormobile.Tcx;

public partial class TcxDocument
{
    /// <summary>
    /// Represents a trackpoint in a lap, containing timestamp, position, altitude, distance, and heart rate data.
    /// </summary>
    public class TcxTrackpoint : GpxDocumentElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TcxTrackpoint"/> class.
        /// </summary>
        /// <param name="element">The XML element representing the trackpoint.</param>
        public TcxTrackpoint(XElement element) : base(element) { }

        /// <summary>
        /// Gets the timestamp when the trackpoint was recorded.
        /// </summary>
        public DateTime Time => (DateTime)_element.Element(_element.GetDefaultNamespace() + "Time")!;

        /// <summary>
        /// Gets the geographic position of the trackpoint as a tuple of latitude and longitude in decimal degrees.
        /// </summary>
        public (double lat, double lon) Position
        {
            get
            {
                var pos = _element.Element(_element.GetDefaultNamespace() + "Position")!;
                return ((double)pos.Element(_element.GetDefaultNamespace() + "LatitudeDegrees")!, (double)pos.Element(_element.GetDefaultNamespace() + "LongitudeDegrees")!);
            }
        }

        /// <summary>
        /// Gets the altitude of the trackpoint in meters above sea level.
        /// </summary>
        public double AltitudeMeters => (double)_element.Element(_element.GetDefaultNamespace() + "AltitudeMeters")!;

        /// <summary>
        /// Gets the cumulative distance traveled up to this trackpoint in meters.
        /// </summary>
        public double DistanceMeters => (double)_element.Element(_element.GetDefaultNamespace() + "DistanceMeters")!;

        /// <summary>
        /// Gets the heart rate in beats per minute (BPM) at this trackpoint.
        /// </summary>
        public double HeartRateBpm => (double)(_element.Element(_element.GetDefaultNamespace() + "HeartRateBpm")!.Element(_element.GetDefaultNamespace() + "Value")!);
    }
}