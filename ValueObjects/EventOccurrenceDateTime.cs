namespace PubgReportCrawler.ValueObjects;

public sealed record EventOccurrenceDateTime(DateTime Value)
{
    public static implicit operator DateTime(EventOccurrenceDateTime dateTime) => dateTime.Value;
}