namespace PubgReportCrawler.Models;

public sealed record Killer(string PubgNick)
{
    public static Killer From(string pubgNick)
    {
        return new Killer(pubgNick);
    }

    public override string ToString()
    {
        return PubgNick;
    }
}
