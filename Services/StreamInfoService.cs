namespace PubgReportCrawler.Services;

using PubgReportCrawler.Entities;
using PubgReportCrawler.ValueObjects;

/// <summary>
/// Represents a service that retrieves stream information for a given account ID and triggers an action whenever new stream info is available.
/// </summary>
/// <param name="pubgReportApi">The IPubgReportApi instance used for accessing streamer interactions.</param>
public class StreamInfoService(IPubgReportApi pubgReportApi)
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
    public async Task GetStreamInfo(PubgReportAccountId accountId, Action<IReadOnlyList<StreamInfo>> onNewStreamInfo)
    {
        var getStreamsResponse = await pubgReportApi.GetStreams(accountId);

        if (getStreamsResponse.Count == 0)
        {
            return;
        }

        List<StreamInfo> infos = [];

        // Extract stream info from response, keyed by match ID
        foreach ((var _, var streamResponses) in getStreamsResponse)
        {
            infos.AddRange(streamResponses
                .Select(streamResponse => StreamInfo.CreateInstance(new StreamerInteractionTimeUtc(streamResponse.TimeEvent)))
            );
        }

        infos = [.. infos.OrderByDescending(si => si.TimeUtc.Value)];

        var lastStreamTimeUtc =
            _lastStreamTimes.GetValueOrDefault(accountId, new StreamerInteractionTimeUtc(DateTime.MinValue));

        var newStreamInfos = infos.Where(si => si.TimeUtc.Value > lastStreamTimeUtc.Value).ToList();

        // No more recent matches in response
        if (newStreamInfos.Count == 0)
        {
            return;
        }

        _lastStreamTimes[accountId] = newStreamInfos.First().TimeUtc;

        onNewStreamInfo(newStreamInfos);
    }
}
