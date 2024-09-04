namespace PubgReportCrawler.Entities;

using PubgReportCrawler.ValueObjects;

/// <summary>
/// Represents the information about a stream.
/// </summary>
public sealed class StreamInfo
{
    /// <summary>
    /// Gets the occurrence date and time of a streamer interaction.
    /// </summary>
    public StreamerInteractionTimeUtc TimeUtc { get; }

    private StreamInfo(StreamerInteractionTimeUtc timeUtc) => this.TimeUtc = timeUtc;

    public static StreamInfo CreateInstance(StreamerInteractionTimeUtc timeUtc) => new(timeUtc);
}
