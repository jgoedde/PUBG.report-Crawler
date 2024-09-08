using OneOf;

namespace PubgReportCrawlerTests.Entities;

using PubgReportCrawler.Entities;

[TestFixture]
[TestOf(typeof(Map))]
public sealed class MapTest
{
    [Test]
    public void CreateWithReadableName_Returns_UnknownMap()
    {
        const string code = "unknown-map";

        OneOf<Map,UnknownMap> createMapResult = Map.CreateWithReadableName(code);

        Assert.Multiple(() =>
        {
            Assert.That(createMapResult.IsT1, Is.True);
            Assert.That(createMapResult.AsT1.CodeName, Is.EqualTo(code));
        });
    }
    
    [Test]
    public void CreateWithReadableName_Returns_NewMap()
    {
        const string code = "Kiki_Main";
        const string name = "Deston";

        OneOf<Map,UnknownMap> createMapResult = Map.CreateWithReadableName(code);

        Assert.Multiple(() =>
        {
            Assert.That(createMapResult.IsT0, Is.True);
            Assert.That(createMapResult.AsT0.Name, Is.EqualTo(name));
        });
    }
}
