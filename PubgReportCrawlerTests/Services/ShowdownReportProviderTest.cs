using Microsoft.Extensions.Options;
using Moq;
using PubgReportCrawler.Config;
using PubgReportCrawler.Models;
using PubgReportCrawler.Services;

namespace PubgReportCrawlerTests.Services;

[TestFixture]
[TestOf(typeof(ShowdownReportProvider))]
public class ShowdownReportProviderTest
{
    private Mock<IOptions<AppOptions>> _mockAppOptions;
    private ShowdownReportProvider _showdownReportProvider;
    private AppOptions _appOptions;

    [SetUp]
    public void SetUp()
    {
        _appOptions = new AppOptions { PubgNick = "TestPlayer", KraftonAccountId = "account.abc" };

        _mockAppOptions = new Mock<IOptions<AppOptions>>();
        _mockAppOptions.Setup(x => x.Value).Returns(_appOptions);

        _showdownReportProvider = new ShowdownReportProvider(_mockAppOptions.Object);
    }

    [Test]
    public void GetMessage_WhenUserIsKiller_ReturnsCorrectMessage()
    {
        // Arrange
        var showdown = new StreamerShowdown(new ShowdownTimeUtc(DateTime.UtcNow),
            FightDetails.Create(Killer.From("TestPlayer"), Victim.From("Streamer123")).AsT0,
            new MatchDetails(Map.MapFactory.CreateWithReadableName("Savage_Main").AsT0,
                GameMode.CreateWithReadableName("duo-fpp").AsT0));

        string expectedMessage =
            "Du hast im Duo auf Sanhok einen Streamer (Streamer123) getötet. Siehe dir den Clip hier an: https://pubg.report/players/account.abc";

        // Act
        var result = _showdownReportProvider.GetMessage(showdown);

        // Assert
        Assert.That(expectedMessage, Is.EqualTo(result));
    }

    [Test]
    public void GetMessage_WhenUserIsVictim_ReturnsCorrectMessage()
    {
        // Arrange
        var showdown = new StreamerShowdown(new ShowdownTimeUtc(DateTime.UtcNow),
            FightDetails.Create(Killer.From("Streamer123"), Victim.From("TestPlayer")).AsT0,
            new MatchDetails(Map.MapFactory.CreateWithReadableName("Desert_Main").AsT0,
                GameMode.CreateWithReadableName("solo-fpp").AsT0));

        string expectedMessage =
            "Du wurdest im Solo auf Miramar von einem Streamer (Streamer123) getötet. Siehe dir den Clip hier an: https://pubg.report/players/account.abc";

        // Act
        var result = _showdownReportProvider.GetMessage(showdown);

        // Assert
        Assert.That(expectedMessage, Is.EqualTo(result));
    }

    // [Test]
    // public void GetMessage_WhenGameModeIsDifferent_ReturnsCorrectMessage()
    // {
    //     // Arrange
    //     var showdown = new StreamerShowdown
    //     {
    //         MatchDetails = (new Map("Sanhok"), new GameMode("Squad")),
    //         FightDetails = new FightDetails
    //         {
    //             KillerName = KillerName.From("AnotherPlayer"), VictimName = "TestPlayer"
    //         }
    //     };
    //
    //     string expectedMessage =
    //         "Du wurdest im Squad auf Sanhok von einem Streamer (AnotherPlayer) getötet. Siehe dir den Clip hier an: https://pubg.report/players/12345";
    //
    //     // Act
    //     var result = _showdownReportProvider.GetMessage(showdown);
    //
    //     // Assert
    //     Assert.AreEqual(expectedMessage, result);
    // }
}
