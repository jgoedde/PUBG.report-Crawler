namespace PubgReportCrawler.ValueObjects;

public sealed record StreamerInteractionTimeUtc(DateTime Value)
{
    public static implicit operator DateTime(StreamerInteractionTimeUtc dateTimeUtc) => dateTimeUtc.Value;
}
