namespace PubgReportCrawler.Models;

/// <summary>
/// Represents a PUBG account ID used for reporting.
/// </summary>
public record PubgReportAccountId(string Value)
{
    public override string ToString() => Value;
}
