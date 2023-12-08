using PubgReportCrawler.Dtos;
using PubgReportCrawler.ValueObjects;
using Refit;

namespace PubgReportCrawler;

/// <summary>
/// Represents the interface for interacting with the PUBG Report API.
/// </summary>
public interface IPubgReportApi
{
    /// <summary>
    /// Retrieves the streams for a given PubgReportAccountId.
    /// </summary>
    /// <param name="id">The PubgReportAccountId of the player.</param>
    /// <returns>An asynchronous operation that returns a GetStreamsResponse object.</returns>
    [Get("/players/{id.Value}/streams")]
    Task<GetStreamsResponse> GetStreams(PubgReportAccountId id);
}