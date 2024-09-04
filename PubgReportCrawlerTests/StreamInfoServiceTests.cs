#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace PubgReportCrawlerTests;
using Moq;

using PubgReportCrawler.Dtos;
using PubgReportCrawler.Entities;
using PubgReportCrawler.Services;
using PubgReportCrawler.ValueObjects;

public class StreamInfoServiceTests
{
    private Mock<IPubgReportApi> _pubgReportApiMock;
    private StreamInfoService _streamInfoService;
    private PubgReportAccountId _accountId;

    [SetUp]
    public void Setup()
    {
        this._pubgReportApiMock = new Mock<IPubgReportApi>();
        this._streamInfoService = new StreamInfoService(this._pubgReportApiMock.Object);

        this._accountId = new PubgReportAccountId("test");
    }

    [Test]
    public async Task GetStreamInfoWhenApiReturnsNoStreamsShouldNotTriggerAction()
    {
        GetStreamsResponse response = [];

        this._pubgReportApiMock.Setup(api => api.GetStreams(this._accountId)).ReturnsAsync(response);

        var actionWasCalled = false;

        await this._streamInfoService.GetStreamInfo(this._accountId, Action);

        Assert.That(actionWasCalled, Is.False);
        this._pubgReportApiMock.Verify(api => api.GetStreams(this._accountId), Times.Once);

        return;

        void Action(IReadOnlyList<StreamInfo> list) => actionWasCalled = true;
    }

    [Test]
    public async Task GetStreamInfoWhenApiReturnsStreamsShouldTriggerAction()
    {
        GetStreamsResponse response = new() { [Guid.NewGuid()] = [new(DateTime.UtcNow)] };

        this._pubgReportApiMock.Setup(api => api.GetStreams(this._accountId)).ReturnsAsync(response);

        var actionWasCalled = false;

        await this._streamInfoService.GetStreamInfo(this._accountId, Action);

        Assert.That(actionWasCalled, Is.True);
        this._pubgReportApiMock.Verify(api => api.GetStreams(this._accountId), Times.Once);

        return;

        void Action(IReadOnlyList<StreamInfo> list) => actionWasCalled = true;
    }

    [Test]
    public async Task GetStreamInfoWhenApiReturnsStreamsShouldTriggerActionWithCorrectStreamInfo()
    {
        StreamResponse streamResponse = new(DateTime.UtcNow);
        GetStreamsResponse response = new() { [Guid.NewGuid()] = [streamResponse] };

        this._pubgReportApiMock.Setup(api => api.GetStreams(this._accountId)).ReturnsAsync(response);

        List<StreamInfo> streamInfos = [];

        await this._streamInfoService.GetStreamInfo(this._accountId, Action);

        Assert.That(streamInfos, Has.Count.EqualTo(1));
        Assert.That(streamInfos[0].TimeUtc.Value, Is.EqualTo(streamResponse.TimeEvent));

        return;

        void Action(IReadOnlyList<StreamInfo> list) => streamInfos.AddRange(list);
    }

    [Test]
    public async Task GetStreamInfoDoesNotTriggerActionWhenNoNewStreamInfoIsAvailable()
    {
        StreamResponse streamResponse = new(DateTime.UtcNow);
        GetStreamsResponse response = new() { [Guid.NewGuid()] = [streamResponse] };

        this._pubgReportApiMock.Setup(api => api.GetStreams(this._accountId)).ReturnsAsync(response);

        List<StreamInfo> streamInfos = [];

        await this._streamInfoService.GetStreamInfo(this._accountId, Action);
        await this._streamInfoService.GetStreamInfo(this._accountId, Action);

        Assert.That(streamInfos, Has.Count.EqualTo(1));
        Assert.That(streamInfos[0].TimeUtc.Value, Is.EqualTo(streamResponse.TimeEvent));

        return;

        void Action(IReadOnlyList<StreamInfo> list) => streamInfos.AddRange(list);
    }
}
