namespace PubgReportCrawler.Entities;

using ValueObjects;

/// <summary>
/// Represents the information about a stream.
/// </summary>
public sealed record StreamerShowdown(StreamerInteractionTimeUtc TimeUtc, FightDetails FightDetails, MatchDetails MatchDetails)
{
    public static StreamerShowdown Create(StreamerInteractionTimeUtc timeUtc, FightDetails fightDetails, MatchDetails matchDetails)
        => new(timeUtc, fightDetails, matchDetails);
}
