namespace PubgReportCrawler.ValueObjects;

/// <summary>
/// Represents a unique identifier for a match.
/// </summary>
public sealed record MatchId(Guid Value)
{
    public static implicit operator Guid(MatchId matchId) => matchId.Value;
    public static implicit operator MatchId(Guid value) => new(value);
}