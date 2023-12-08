using PubgReportCrawler.Dtos;
using PubgReportCrawler.Entities;
using PubgReportCrawler.ValueObjects;

namespace PubgReportCrawler.Services;

/// <summary>
/// Represents a service that retrieves stream information for a given account ID and triggers an action whenever new stream info is available.
/// </summary>
/// <param name="pubgReportApi">The IPubgReportApi instance used for accessing streamer interactions.</param>
public class StreamInfoService(IPubgReportApi pubgReportApi)
{
    /// <summary>
    /// Stores the last streamer interaction time for each PubgReportAccountId.
    /// </summary>
    private readonly Dictionary<PubgReportAccountId, EventOccurrenceDateTime> _lastStreamTimes = new();

    /// <summary>
    /// Retrieves stream information for a given account ID and triggers an action whenever new stream info is available.
    /// </summary>
    /// <param name="accountId">The PubgReportAccountId of the account to retrieve stream information for.</param>
    /// <param name="onNewStreamInfo">The action to be triggered whenever a new streamer interaction info is available.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task GetStreamInfo(PubgReportAccountId accountId, Action<IReadOnlyList<StreamInfo>> onNewStreamInfo)
    {
        GetStreamsResponse getStreamsResponse = await pubgReportApi.GetStreams(accountId);

        if (getStreamsResponse.Count == 0)
        {
            return;
        }

        List<StreamInfo> infos = new();

        // Extract stream info from response, keyed by match ID
        foreach ((Guid matchId, List<StreamResponse> list) in getStreamsResponse)
        {
            infos.AddRange(list
                .Select(streamResponse =>
                {
                    StreamId sId = new(streamResponse.ID);
                    EventOccurrenceDateTime t = new(streamResponse.TimeEvent);
                    MatchId mId = new(matchId);

                    return StreamInfo.CreateInstance(sId, mId, t);
                })
            );
        }

        infos = infos.OrderByDescending(si => si.Time.Value).ToList();

        EventOccurrenceDateTime lastStreamTime =
            _lastStreamTimes.GetValueOrDefault(accountId, new EventOccurrenceDateTime(DateTime.MinValue));

        List<StreamInfo> newStreamInfos = infos.Where(si => si.Time.Value > lastStreamTime.Value).ToList();

        // No more recent matches in response
        if (!newStreamInfos.Any())
        {
            return;
        }

        _lastStreamTimes[accountId] = newStreamInfos.First().Time;

        onNewStreamInfo(newStreamInfos);
    }
}