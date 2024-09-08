using OneOf;
using PubgReportCrawler.ValueObjects;

namespace PubgReportCrawler.Entities;

public sealed record FightDetails(KillerName KillerName, VictimName VictimName)
{
    public static OneOf<FightDetails, VictimAndKillerCannotBeTheSame> Create(KillerName killerName, VictimName victimName)
    {
        if (killerName.Name.Equals(victimName.Name))
        {
            return new VictimAndKillerCannotBeTheSame(killerName.Name);
        }

        return new FightDetails(killerName, victimName);
    }
}

public sealed record VictimAndKillerCannotBeTheSame(string Name);
