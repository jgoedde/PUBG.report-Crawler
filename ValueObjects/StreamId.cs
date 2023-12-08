namespace PubgReportCrawler.ValueObjects;

public sealed record StreamId(string Value)
{
    public static implicit operator string(StreamId streamId) => streamId.Value;
    public static implicit operator StreamId(string value) => new(value);
}