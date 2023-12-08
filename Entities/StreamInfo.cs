using PubgReportCrawler.ValueObjects;

namespace PubgReportCrawler.Entities;

/// <summary>
/// Represents the information about a stream.
/// </summary>
public sealed class StreamInfo
{
    /// <summary>
    /// Gets the ID of the Stream (obtained from pubg.report API).
    /// </summary>
    public StreamId ID { get; }

    /// <summary>
    /// Gets the unique identifier for a match.
    /// </summary>
    public MatchId MatchId { get; }

    /// <summary>
    /// Gets the occurrence date and time of a streamer interaction.
    /// </summary>
    public EventOccurrenceDateTime Time { get; }

    private StreamInfo(StreamId id, MatchId matchId, EventOccurrenceDateTime time)
    {
        ID = id;
        MatchId = matchId;
        Time = time;
    }

    public static StreamInfo CreateInstance(StreamId id, MatchId matchId, EventOccurrenceDateTime time)
    {
        return new StreamInfo(id, matchId, time);
    }
}