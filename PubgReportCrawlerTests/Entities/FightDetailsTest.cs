using PubgReportCrawler.Models;

namespace PubgReportCrawlerTests.Entities;

[TestFixture]
[TestOf(typeof(FightDetails))]
public class FightDetailsTest
{
    [Test]
    public void CreateFightDetails_ReturnsError_WithSameNames()
    {
        var victimName = "shroud";
        var killerName = "shroud";

        var createResult = FightDetails.Create(new Killer(victimName), new Victim(killerName));
        Assert.Multiple(() =>
        {
            Assert.That(createResult.IsT1, Is.True);
            Assert.That(createResult.AsT1.Name, Is.EqualTo(victimName));
        });
    }
}
