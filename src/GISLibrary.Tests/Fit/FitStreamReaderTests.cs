using System.Text;
using Tudormobile.GIS.Fit;

namespace GISLibrary.Tests.Fit;

[TestClass]
public class FitStreamReaderTests
{
    [TestMethod]
    public void EmptyStream_ReturnsDefaultName()
    {
        var ms = new MemoryStream();
        var reader = new FitStreamReader(ms, Encoding.UTF8);

        Assert.AreEqual("No Name", reader.FriendlyName);
    }

    [TestMethod]
    public void EmptyStream_HeaderPropertyThrows()
    {
        var ms = new MemoryStream();
        var reader = new FitStreamReader(ms, Encoding.UTF8, leaveOpen: true);

        Assert.ThrowsExactly<FormatException>(() => reader.Header);
    }

    [TestMethod]
    public async Task CreateAsync_CreatesReader()
    {
        var path = "TestData/sample.fit";
        var contents = await File.ReadAllBytesAsync(path, TestContext.CancellationToken);
        using var ms = new MemoryStream(contents);
        using var reader = await FitStreamReader.CreateAsync(ms, leaveOpen: true, cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(reader);
        Assert.IsNotNull(reader.ReadHeader());
        Assert.IsNotEmpty(await reader.ReadRecordsAsync(TestContext.CancellationToken).ToArrayAsync(TestContext.CancellationToken));

    }

    [TestMethod]
    public async Task CreateAsyncWithGzipData_CreatesReader()
    {
        var path = "TestData/sample.fit.gz";
        var contents = await File.ReadAllBytesAsync(path, TestContext.CancellationToken);
        using var ms = new MemoryStream(contents);
        using var reader = await FitStreamReader.CreateAsync(ms, leaveOpen: false, cancellationToken: TestContext.CancellationToken);
        Assert.IsNotNull(reader);
        Assert.IsNotNull(await reader.ReadHeaderAsync(TestContext.CancellationToken));
        Assert.IsNotEmpty(await reader.ReadRecordsAsync(TestContext.CancellationToken).ToArrayAsync(TestContext.CancellationToken));
    }


    public TestContext TestContext { get; set; }
}
