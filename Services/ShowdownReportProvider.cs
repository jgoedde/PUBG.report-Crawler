using Microsoft.Extensions.Options;
using PubgReportCrawler.Config;
using PubgReportCrawler.Models;

namespace PubgReportCrawler.Services;

public sealed class ShowdownReportProvider(IOptions<AppOptions> appOptions)
{
    private readonly AppOptions _appOptions = appOptions.Value;

    public string GetMessage(StreamerShowdown showdown)
    {
        (Map map, GameMode gameMode) = showdown.MatchDetails;
        FightDetails fightDetails = showdown.FightDetails;

        string callToAction =
            $"Siehe dir den Clip hier an: https://pubg.report/players/{_appOptions.KraftonAccountId}";

        var didUserKillStreamer = fightDetails.Killer.Equals(Killer.From(_appOptions.PubgNick));

        if (didUserKillStreamer)
        {
            return $"Du hast im {gameMode.ToCapitalizedString()} auf {map} einen Streamer ({fightDetails.Victim}) getötet. {callToAction}";
        }

        return $"Du wurdest im {gameMode.ToCapitalizedString()} auf {map} von einem Streamer ({fightDetails.Killer}) getötet. {callToAction}";
    }

    public string GetManyMoreShowdownsMessage()
    {
        return $"Du bist in der letzten Zeit vielen Streamern begegnet. Schau dir hier die Clips an: https://pubg.report/players/{_appOptions.KraftonAccountId}";
    }
}
