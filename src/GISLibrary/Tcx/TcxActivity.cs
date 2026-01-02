using System.Xml.Linq;

namespace Tudormobile.Tcx;

public partial class TcxDocument
{
    /// <summary>
    /// Represents an activity in a TCX document, containing sport type, identifier, and lap data.
    /// </summary>
    public class TcxActivity : GpxDocumentElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TcxActivity"/> class.
        /// </summary>
        /// <param name="activityElement">The XML element representing the activity.</param>
        public TcxActivity(XElement activityElement) : base(activityElement) { }

        /// <summary>
        /// Gets the unique identifier of the activity as a string in ISO 8601 format.
        /// </summary>
        public string Id => _element.Element(_element.GetDefaultNamespace() + "Id")?.Value ?? DateTime.MinValue.ToString("O");

        /// <summary>
        /// Gets the activity identifier as a <see cref="DateTime"/>.
        /// </summary>
        public DateTime ActivityId => DateTime.Parse(Id);

        /// <summary>
        /// Gets the sport type of the activity (e.g., "Running", "Biking").
        /// </summary>
        public string Sport => _element.Attribute("Sport")?.Value ?? "Unknown";

        /// <summary>
        /// Gets the collection of laps in the activity.
        /// </summary>
        public IEnumerable<TcxActivityLap> Laps => _element.Elements(_element.GetDefaultNamespace() + "Lap").Select(x => new TcxActivityLap(x));
    }
}