using Tudormobile.GIS.Fit;

namespace GISLibrary.Tests.Fit;

[TestClass]
public class FitFieldDefinitionTests
{
    [TestMethod]
    public void Constructor_SetsProperties()
    {
        var field = new FitFieldDefinition(253, 4, 0x86);

        Assert.AreEqual((byte)253, field.FieldDefinitionNumber);
        Assert.AreEqual((byte)4, field.Size);
        Assert.AreEqual((byte)0x86, field.BaseType);
    }
}
