using System.Collections.Immutable;
using OneOf;

namespace PubgReportCrawler.Models;

public sealed record Map
{
    public string Name { get; }

    private Map(string name)
    {
        Name = name;
    }

    public override string ToString() => Name;

    // Factory method for creating maps
    public static class MapFactory
    {
        private static readonly ImmutableDictionary<string, string> MapDictionary = ImmutableDictionary.CreateRange(new Dictionary<string, string>
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
        });

        public static OneOf<Map, UnknownMap> CreateWithReadableName(string codeName)
        {
            return MapDictionary.TryGetValue(codeName, out var humanReadableMapName)
                ? new Map(humanReadableMapName)
                : new UnknownMap(codeName);
        }
    }
}

public sealed record UnknownMap(string CodeName);
