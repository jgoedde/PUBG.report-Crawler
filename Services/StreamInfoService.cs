using Microsoft.Extensions.Logging;
using PubgReportCrawler.Dtos;
using PubgReportCrawler.Entities;
using PubgReportCrawler.ValueObjects;

namespace PubgReportCrawler.Services;

/// <summary>
/// Represents a service that retrieves stream information for a given account ID and triggers an action whenever new stream info is available.
/// </summary>
/// <param name="pubgReportApi">The IPubgReportApi instance used for accessing streamer interactions.</param>
public sealed class StreamInfoService(IPubgReportApi pubgReportApi, ILogger<StreamInfoService> logger)
{
    /// <summary>
    /// Stores the last streamer interaction time for each PubgReportAccountId.
    /// </summary>
    private readonly Dictionary<PubgReportAccountId, StreamerInteractionTimeUtc> _lastStreamTimes = [];

    /// <summary>
    /// Retrieves stream information for a given account ID and triggers an action whenever new stream info is available.
    /// </summary>
    /// <param name="accountId">The PubgReportAccountId of the account to retrieve stream information for.</param>
    /// <param name="onNewStreamInfo">The action to be triggered whenever a new streamer interaction info is available.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task GetStreamInfo(PubgReportAccountId accountId, Action<IReadOnlyList<StreamerShowdown>> onNewStreamInfo)
    {
        var getStreamsResponse = await pubgReportApi.GetStreams(accountId);

        if (!getStreamsResponse.Any())
        {
            return;
        }

        var validStreamInfos = getStreamsResponse
            .SelectMany(kvp => kvp.Value)
            .Select(CreateStreamerShowdown)
            .OfType<StreamerShowdown>()
            .OrderByDescending(si => si.TimeUtc.Value)
            .ToList();

        var lastStreamTimeUtc = _lastStreamTimes.GetValueOrDefault(accountId, new StreamerInteractionTimeUtc(DateTime.MinValue));

        var newStreamInfos = validStreamInfos
            .Where(si => si.TimeUtc.Value > lastStreamTimeUtc.Value)
            .ToList();

        if (newStreamInfos.Count == 0)
        {
            return;
        }

        _lastStreamTimes[accountId] = newStreamInfos.First().TimeUtc;
        onNewStreamInfo(newStreamInfos);
    }

    /// <summary>
    /// Creates a StreamerShowdown object if valid, or returns null if invalid.
    /// </summary>
    private StreamerShowdown? CreateStreamerShowdown(StreamResponse streamResponse)
    {
        var createMapResult = Map.CreateWithReadableName(streamResponse.Map);
        var createGameModeResult = GameMode.CreateWithReadableName(streamResponse.Mode);

        if (createMapResult.TryPickT1(out var unknownMap, out _))
        {
            logger.LogWarning($"Encountered an unknown map code ({unknownMap.CodeName}). Skipping...");
            return null;
        }

        if (createGameModeResult.TryPickT1(out var unknownGameMode, out _))
        {
            logger.LogWarning($"Encountered an unknown game mode code ({unknownGameMode.CodeName}). Skipping...");
            return null;
        }

        var killer = KillerName.From(streamResponse.Killer);
        var victim = VictimName.From(streamResponse.Victim);
        var fightDetails = FightDetails.Create(killer, victim);

        if (fightDetails.TryPickT1(out var error, out _))
        {
            logger.LogError($"Invalid victim and killer constellation ({error.Name}). Skipping stream...");
            return null;
        }

        var matchDetails = new MatchDetails(createMapResult.AsT0, createGameModeResult.AsT0);
        var dateTime = new StreamerInteractionTimeUtc(streamResponse.TimeEvent);

        return StreamerShowdown.Create(dateTime, fightDetails.AsT0, matchDetails);
    }
}
