namespace PubgReportCrawler.ValueObjects;

public sealed record StreamId(string Value)
{
    public static implicit operator string(StreamId streamId)
    {
        return streamId.Value;
    }

    public static implicit operator StreamId(string value)
    {
        return new StreamId(value);
    }
}