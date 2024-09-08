using OneOf;

namespace PubgReportCrawler.Models;

public sealed record FightDetails(Killer Killer, Victim Victim)
{
    public static OneOf<FightDetails, VictimAndKillerCannotBeTheSame> Create(Killer killer, Victim victim)
    {
        if (killer.PubgNick.Equals(victim.PubgNick))
        {
            return new VictimAndKillerCannotBeTheSame(killer.PubgNick);
        }

        return new FightDetails(killer, victim);
    }
}

public sealed record VictimAndKillerCannotBeTheSame(string Name);
