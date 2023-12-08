using Moq;

using PubgReportCrawler.Dtos;
using PubgReportCrawler.Entities;
using PubgReportCrawler.Services;
using PubgReportCrawler.ValueObjects;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace PubgReportCrawlerTests;

public class StreamInfoServiceTests
{
    private Mock<IPubgReportApi> _pubgReportApiMock;
    private StreamInfoService _streamInfoService;
    private PubgReportAccountId _accountId;

    [SetUp]
    public void Setup()
    {
        _pubgReportApiMock = new Mock<IPubgReportApi>();
        _streamInfoService = new StreamInfoService(_pubgReportApiMock.Object);

        _accountId = new PubgReportAccountId("test");
    }

    [Test]
    public async Task GetStreamInfo_WhenApiReturnsNoStreams_ShouldNotTriggerAction()
    {
        GetStreamsResponse response = new();

        _pubgReportApiMock.Setup(api => api.GetStreams(_accountId)).ReturnsAsync(response);

        bool actionWasCalled = false;

        await _streamInfoService.GetStreamInfo(_accountId, Action);

        Assert.That(actionWasCalled, Is.False);
        _pubgReportApiMock.Verify(api => api.GetStreams(_accountId), Times.Once);

        return;

        void Action(IReadOnlyList<StreamInfo> list)
        {
            actionWasCalled = true;
        }
    }

    [Test]
    public async Task GetStreamInfo_WhenApiReturnsStreams_ShouldTriggerAction()
    {
        GetStreamsResponse response = new() { [Guid.NewGuid()] = new List<StreamResponse> { new(DateTime.UtcNow) } };

        _pubgReportApiMock.Setup(api => api.GetStreams(_accountId)).ReturnsAsync(response);

        bool actionWasCalled = false;

        await _streamInfoService.GetStreamInfo(_accountId, Action);

        Assert.That(actionWasCalled, Is.True);
        _pubgReportApiMock.Verify(api => api.GetStreams(_accountId), Times.Once);

        return;

        void Action(IReadOnlyList<StreamInfo> list)
        {
            actionWasCalled = true;
        }
    }

    [Test]
    public async Task GetStreamInfo_WhenApiReturnsStreams_ShouldTriggerActionWithCorrectStreamInfo()
    {
        StreamResponse streamResponse = new(DateTime.UtcNow);
        GetStreamsResponse response = new() { [Guid.NewGuid()] = new List<StreamResponse> { streamResponse } };

        _pubgReportApiMock.Setup(api => api.GetStreams(_accountId)).ReturnsAsync(response);

        List<StreamInfo> streamInfos = new();

        await _streamInfoService.GetStreamInfo(_accountId, Action);

        Assert.That(streamInfos, Has.Count.EqualTo(1));
        Assert.That(streamInfos[0].TimeUtc.Value, Is.EqualTo(streamResponse.TimeEvent));

        return;

        void Action(IReadOnlyList<StreamInfo> list)
        {
            streamInfos.AddRange(list);
        }
    }

    [Test]
    public async Task GetStreamInfo_DoesNotTriggerAction_WhenNoNewStreamInfoIsAvailable()
    {
        StreamResponse streamResponse = new(DateTime.UtcNow);
        GetStreamsResponse response = new() { [Guid.NewGuid()] = new List<StreamResponse> { streamResponse } };

        _pubgReportApiMock.Setup(api => api.GetStreams(_accountId)).ReturnsAsync(response);

        List<StreamInfo> streamInfos = new();

        await _streamInfoService.GetStreamInfo(_accountId, Action);
        await _streamInfoService.GetStreamInfo(_accountId, Action);

        Assert.That(streamInfos, Has.Count.EqualTo(1));
        Assert.That(streamInfos[0].TimeUtc.Value, Is.EqualTo(streamResponse.TimeEvent));

        return;

        void Action(IReadOnlyList<StreamInfo> list)
        {
            streamInfos.AddRange(list);
        }
    }
}