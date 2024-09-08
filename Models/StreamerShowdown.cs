namespace PubgReportCrawler.Models;

/// <summary>
/// Represents a fight between the user and a streamer in PUBG.
/// </summary>
/// <param name="ShowdownTimeUtc">The date and time at which the fight took place.</param>
/// <param name="FightDetails">Details about the fight including the killer and the victim.</param>
/// <param name="MatchDetails">Details about the match including the map and the game mode.</param>
public sealed record StreamerShowdown(
    ShowdownTimeUtc ShowdownTimeUtc,
    FightDetails FightDetails,
    MatchDetails MatchDetails);
