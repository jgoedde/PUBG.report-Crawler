namespace PubgReportCrawler.Models;

public sealed record Victim(string PubgNick)
{
    public static Victim From(string pubgNick)
    {
        return new Victim(pubgNick);
    }

    public override string ToString()
    {
        return PubgNick;
    }
}
