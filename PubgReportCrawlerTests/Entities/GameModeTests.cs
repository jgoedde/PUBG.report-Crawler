using OneOf;
using PubgReportCrawler.ValueObjects;

namespace PubgReportCrawlerTests.Entities;

[TestFixture]
[TestOf(typeof(GameMode))]
public sealed class GameModeTests
{
    [Test]
    [TestCase("duo-fpp", "duo")]
    [TestCase("solo-tpp", "solo")]
    [TestCase("squad-tpp", "squad")]
    public void CreateGameMode_ReturnsCorrectGameMode(string input, string? expectedOutput)
    {
        OneOf<GameMode, UnknownGameMode> createGameModeResult = GameMode.CreateWithReadableName(input);

        Assert.That(createGameModeResult.IsT0, Is.True);
        Assert.That(createGameModeResult.IsT1, Is.False);
        Assert.That(createGameModeResult.AsT0.Value, Is.EqualTo(expectedOutput));
    }

    [Test]
    public void CreateGameMode_ReturnsError()
    {
        const string input = "some-unknown-gamemode";

        OneOf<GameMode, UnknownGameMode> createGameModeResult = GameMode.CreateWithReadableName(input);

        Assert.That(createGameModeResult.IsT0, Is.False);
        Assert.That(createGameModeResult.IsT1, Is.True);
        Assert.That(createGameModeResult.AsT1.CodeName, Is.EqualTo(input));
    }
}
