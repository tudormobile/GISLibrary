using Tudormobile.GIS.Fit;

namespace GISLibrary.Tests.Fit;

[TestClass]
public class FitDeveloperFieldDefinitionTests
{
    [TestMethod]
    public void Constructor_SetsProperties()
    {
        var field = new FitDeveloperFieldDefinition(0, 4, 1);

        Assert.AreEqual((byte)0, field.FieldDefinitionNumber);
        Assert.AreEqual((byte)4, field.Size);
        Assert.AreEqual((byte)1, field.DeveloperDataIndex);
    }
}
