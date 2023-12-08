namespace PubgReportCrawler.ValueObjects;

/// <summary>
/// Represents a PUBG account ID used for reporting.
/// </summary>
public record PubgReportAccountId(string Value)
{
    public static implicit operator string(PubgReportAccountId accountId) => accountId.Value;
    public static implicit operator PubgReportAccountId(string value) => new(value);
}