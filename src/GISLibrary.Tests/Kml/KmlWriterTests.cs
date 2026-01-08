using System.Text;
using Tudormobile.Kml;

namespace GISLibrary.Tests.Kml;

[TestClass]
public class KmlWriterTests
{
    [TestMethod]
    public async Task WriteStartKmlAsync_WritesIdNameDescription()
    {
        // Arrange
        var id = "some id";
        var name = "some name";
        var description = "some description";
        using var stream = new MemoryStream();
        using var writer = KmlWriter.Create(stream);

        // Act
        await writer.WriteStartKmlAsync(id, name, description);
        await writer.WriteEndKmlAsync();

        // Assert
        var content = Encoding.UTF8.GetString(stream.ToArray());
        Assert.Contains(id, content);
        Assert.Contains(name, content);
        Assert.Contains(description, content);
    }

    [TestMethod]
    public async Task WriteStartKmlAsync_WritesIdName()
    {
        // Arrange
        var id = "some id";
        var name = "some name";
        using var stream = new MemoryStream();
        using var writer = KmlWriter.Create(stream);

        // Act
        await writer.WriteStartKmlAsync(id, name);
        await writer.FlushAsync();

        // Assert
        var content = Encoding.UTF8.GetString(stream.ToArray());
        Assert.Contains(id, content);
        Assert.Contains(name, content);
    }


    [TestMethod]
    public async Task WriteStartKmlAsync_WithHtmlDescription_WritesDescriptionAsCdata()
    {
        // Arrange
        var description = "<h1>description contains html</h1>";
        using var stream = new MemoryStream();
        using var writer = KmlWriter.Create(stream);

        // Act
        await writer.WriteStartKmlAsync(description: description);
        await writer.WriteEndKmlAsync();

        // Assert
        var content = Encoding.UTF8.GetString(stream.ToArray());
        Assert.Contains("CDATA", content);
        Assert.Contains(description, content);
        Assert.Contains("<Document>", content);
        Assert.Contains("</Document>", content);
    }

    [TestMethod]
    public async Task WriteStartKmlAsync_WithNoIdNameDescription_WritesRootTagOnly()
    {
        // Arrange
        using var stream = new MemoryStream();
        using var writer = KmlWriter.Create(stream);

        // Act
        await writer.WriteStartKmlAsync();
        await writer.FlushAsync();

        // Assert
        var content = Encoding.UTF8.GetString(stream.ToArray());
        Assert.Contains("<kml", content);
        Assert.DoesNotContain("<Document>", content);
        Assert.DoesNotContain("</Document>", content);
    }

    [TestMethod]
    public async Task WriteFoldersAsync_WritesFolderElements()
    {
        // Arrange
        var folders = new List<KmlFolder>()
        {
            new (new KmlFolderItem("id1","name1","description1")),
            new (new KmlFolderItem("id2","name2", string.Empty)),
            new (new KmlFolderItem("id3", string.Empty, string.Empty)),
            new (new KmlFolderItem(string.Empty, string.Empty, string.Empty)),
        };
        using var stream = new MemoryStream();
        using var writer = KmlWriter.Create(stream);

        // Act
        await writer.WriteStartKmlAsync();
        await writer.WriteFoldersAsync(folders, TestContext.CancellationToken);
        await writer.FlushAsync();

        // Assert
        var content = Encoding.UTF8.GetString(stream.ToArray());
        Assert.Contains("id1", content);
        Assert.Contains("id2", content);
        Assert.Contains("id3", content);
        Assert.Contains("name1", content);
        Assert.Contains("name2", content);
        Assert.Contains("description1", content);
    }

    [TestMethod]
    public async Task WritePlacemarksAsync_WritesPlacemarksElements()
    {
        // Arrange
        var placemarks = new List<KmlPlacemark>()
        {
            new (new KmlFolderItem("id1","name1","description1")),
            new (new KmlFolderItem("id2","name2", string.Empty)),
            new (new KmlFolderItem("id3", string.Empty, string.Empty)),
            new (new KmlFolderItem(string.Empty, string.Empty, string.Empty)),
        };
        using var stream = new MemoryStream();
        using var writer = KmlWriter.Create(stream);

        // Act
        await writer.WriteStartKmlAsync();
        await writer.WritePlacemarksAsync(placemarks, TestContext.CancellationToken);
        await writer.FlushAsync();

        // Assert
        var content = Encoding.UTF8.GetString(stream.ToArray());
        Assert.Contains("id1", content);
        Assert.Contains("id2", content);
        Assert.Contains("id3", content);
        Assert.Contains("name1", content);
        Assert.Contains("name2", content);
        Assert.Contains("description1", content);
    }

    public TestContext TestContext { get; set; }
}
