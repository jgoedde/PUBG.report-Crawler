namespace PubgReportCrawler.Dtos;

// ReSharper disable once ClassNeverInstantiated.Global
public sealed class StreamResponse
{
    public int AttackID { get; init; }

    public string ID { get; init; }

    public DateTime TimeEvent { get; init; }

    public string VideoID { get; init; }
}