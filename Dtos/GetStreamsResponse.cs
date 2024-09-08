namespace PubgReportCrawler.Dtos;

/// <summary>
/// Represents the response model of a streamer interaction.
/// </summary>
/// <param name="TimeEvent">When a streamer interaction occurred.</param>
/// <param name="Mode">The code of the game mode of the match. I.e. duo-fpp</param>
/// <param name="Map">The code of the map. I.e. Kiki_Main</param>
/// <param name="TwitchID">The streamer's twitch channel ID.</param>
/// <param name="Killer">The killer's PUBG nickname of the interaction.</param>
/// <param name="Victim">The victim's PUBG nickname of the interaction.</param>
public sealed record StreamResponse(DateTime TimeEvent, string Mode, string Map, int TwitchID, string Killer, string Victim);

/// <summary>
/// Represents a dictionary of recent streamer interactions.
/// </summary>
public sealed class GetStreamsResponse : Dictionary<Guid, List<StreamResponse>>;

/*
Example response:

{
    "294aed47-ee3c-4a83-b778-edadddcd613e": [
        {
            "ID": "294aed47-ee3c-4a83-b778-edadddcd613e-1509959919",
            "AttackID": 1509959919,
            "TwitchID": 113204201,
            "MixerID": "",
            "DamageCauser": "WeapDragunov_C",
            "TimeEvent": "2024-09-01T19:07:41Z",
            "VideoID": "v2239992251",
            "MatchID": "294aed47-ee3c-4a83-b778-edadddcd613e",
            "Victim": "Hasgarson",
            "Killer": "Kasiux-",
            "Map": "Neon_Main",
            "Mode": "squad-fpp",
            "TimeDiff": "00:38:05",
            "Date": "Sep 1, 2024",
            "Event": "LogTeammateMakeGroggy",
            "Rank": 0,
            "RoundsPlayed": 0,
            "Distance": 96,
            "ExpiresAt": 0
        }
    ]
}

*/
