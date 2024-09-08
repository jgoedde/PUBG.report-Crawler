using OneOf;

namespace PubgReportCrawler.Entities;

public sealed record Map(string Name)
{
    private static readonly IReadOnlyDictionary<string, string> mapDictionary = new Dictionary<string, string>
    {
        { "Baltic_Main", "Erangel (Remastered)" },
        { "Chimera_Main", "Paramo" },
        { "Desert_Main", "Miramar" },
        { "DihorOtok_Main", "Vikendi" },
        { "Erangel_Main", "Erangel" },
        { "Heaven_Main", "Haven" },
        { "Kiki_Main", "Deston" },
        { "Range_Main", "Camp Jackal" },
        { "Savage_Main", "Sanhok" },
        { "Summerland_Main", "Karakin" },
        { "Tiger_Main", "Taego" },
        { "Neon_Main", "Rondo" }
    };

    public static OneOf<Map, UnknownMap> CreateWithReadableName(string codeName) =>
        mapDictionary.TryGetValue(codeName, out var humanReadableMapName)
            ? new Map(humanReadableMapName)
            : new UnknownMap(codeName);

    public override string ToString()
    {
        return Name;
    }
}

public sealed record UnknownMap(string CodeName);
