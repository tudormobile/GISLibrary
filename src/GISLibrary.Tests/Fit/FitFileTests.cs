using Tudormobile.GIS.Fit;

namespace GISLibrary.Tests.Fit;

[TestClass]
public class FitFileTests
{
    [TestMethod]
    public void ReadHeader_FromUncompressedFitFile_ReturnsHeaderMatchingFileSize()
    {
        var path = "TestData/sample.fit";
        var fileSize = new FileInfo(path).Length;
        var file = new FitFile(path);
        using var reader = file.CreateReader();
        var header = reader.ReadHeader();

        Assert.AreEqual(14, header.HeaderSize);
        Assert.AreEqual(fileSize, header.HeaderSize + header.DataSize + 2 /* 2-byte crc at end of file */);
        Assert.IsNotNull(reader.Header);

    }

    [TestMethod]
    public void ReadHeader_FromGZipCompressedFitFile_ReturnsHeaderMatchingUncompressedSize()
    {
        var path = "TestData/sample.fit.gz";

        // Independently decompress to determine the true uncompressed size,
        // so the assertion below is derived from the test file itself rather
        // than a precomputed/hardcoded value. This keeps the test valid even
        // if the test data file is replaced.
        long uncompressedSize;
        using (var fileStream = File.OpenRead(path))
        using (var gzipStream = new System.IO.Compression.GZipStream(fileStream, System.IO.Compression.CompressionMode.Decompress))
        using (var countingStream = new MemoryStream())
        {
            gzipStream.CopyTo(countingStream);
            uncompressedSize = countingStream.Length;
        }

        var file = new FitFile(path);
        using var reader = file.CreateReader();
        var header = reader.ReadHeader();

        Assert.AreEqual(14, header.HeaderSize);
        Assert.AreEqual(uncompressedSize, header.HeaderSize + header.DataSize + 2 /* 2-byte crc at end of file */);
    }

    [TestMethod]
    public async Task CreateReader_CanReadAllRecordsAsynchronously()
    {
        var path = "TestData/sample.fit.gz";
        var file = new FitFile(path);
        var reader = file.CreateReader();

        var actual = await reader.ReadRecordsAsync(TestContext.CancellationToken).ToListAsync(TestContext.CancellationToken);

        Assert.IsNotNull(actual);
        Assert.IsNotEmpty(actual);
        Assert.HasCount(2603, actual);

        Assert.AreEqual("Bill Tudor", reader.FriendlyName);
    }

    [TestMethod]
    public async Task CreateReader_CanReadAllTrackpointsAsynchronously()
    {
        var path = "TestData/sample.fit";
        var file = new FitFile(path);
        var reader = file.CreateReader();

        var actual = await reader.ReadRecordsAsync(TestContext.CancellationToken)
            .Where(x => x.IsRecordMessage())
            .Select(x => x.DecodeTrackpoint())
            .ToListAsync(TestContext.CancellationToken);

        Assert.IsNotEmpty(actual);
        Assert.HasCount(2598, actual);
        Assert.AreEqual(42.84601, actual[0].Position!.Latitude, 0.0001);
        Assert.AreEqual(-73.82947, actual[0].Position!.Longitude, 0.0001);
        Assert.AreEqual(76.6, actual[0].Position!.Altitude!.Value, 0.0001);
        Assert.AreEqual(new DateTime(2021, 4, 4, 18, 1, 13, DateTimeKind.Utc), actual[0].Timestamp);


    }

    public TestContext TestContext { get; set; }    // Set by MSTEST
}
