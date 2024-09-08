using PubgReportCrawler.Models;

namespace PubgReportCrawlerTests;

using Microsoft.Extensions.Logging;

using Moq;

using PubgReportCrawler.Dtos;
using PubgReportCrawler.Services;

public class StreamInfoServiceTests
{
    private Mock<IPubgReportApi> _pubgReportApiMock;
    private Mock<ILogger<StreamInfoService>> _loggerMock;
    private StreamInfoService _streamInfoService;
    private PubgReportAccountId _accountId;

    [SetUp]
    public void Setup()
    {
        _pubgReportApiMock = new Mock<IPubgReportApi>();
        _loggerMock = new Mock<ILogger<StreamInfoService>>();
        _streamInfoService = new StreamInfoService(_pubgReportApiMock.Object, _loggerMock.Object);

        _accountId = new PubgReportAccountId("test");
    }

    [Test]
    public async Task GetStreamInfoWhenApiReturnsNoStreamsShouldNotTriggerAction()
    {
        GetStreamsResponse response = [];

        _pubgReportApiMock.Setup(api => api.GetStreams(_accountId)).ReturnsAsync(response);

        var actionWasCalled = false;

        await _streamInfoService.GetStreamInfo(_accountId, Action);

        Assert.That(actionWasCalled, Is.False);
        _pubgReportApiMock.Verify(api => api.GetStreams(_accountId), Times.Once);

        return;

        void Action(IReadOnlyList<StreamerShowdown> list) => actionWasCalled = true;
    }

    [Test]
    public async Task GetStreamInfoWhenApiReturnsStreamsShouldTriggerAction()
    {
        GetStreamsResponse response = new() { [Guid.NewGuid()] = [new StreamResponse(DateTime.UtcNow, "duo-fpp", "Kiki_Main", 0, "Foo", "Bar")] };

        _pubgReportApiMock.Setup(api => api.GetStreams(_accountId)).ReturnsAsync(response);

        var actionWasCalled = false;

        await _streamInfoService.GetStreamInfo(_accountId, Action);

        Assert.That(actionWasCalled, Is.True);
        _pubgReportApiMock.Verify(api => api.GetStreams(_accountId), Times.Once);

        return;

        void Action(IReadOnlyList<StreamerShowdown> list) => actionWasCalled = true;
    }

    [Test]
    public async Task GetStreamInfoWhenApiReturnsStreamsShouldTriggerActionWithCorrectStreamInfo()
    {
        StreamResponse streamResponse = new(DateTime.UtcNow, "duo-fpp", "Kiki_Main", 0, "Foo", "Bar");
        GetStreamsResponse response = new() { [Guid.NewGuid()] = [streamResponse] };

        _pubgReportApiMock.Setup(api => api.GetStreams(_accountId)).ReturnsAsync(response);

        List<StreamerShowdown> streamInfos = [];

        await _streamInfoService.GetStreamInfo(_accountId, Action);

        Assert.That(streamInfos, Has.Count.EqualTo(1));
        Assert.That(streamInfos[0].ShowdownTimeUtc.Value, Is.EqualTo(streamResponse.TimeEvent));

        return;

        void Action(IReadOnlyList<StreamerShowdown> list) => streamInfos.AddRange(list);
    }

    [Test]
    public async Task GetStreamInfoDoesNotTriggerActionWhenNoNewStreamInfoIsAvailable()
    {
        StreamResponse streamResponse = new(DateTime.UtcNow, "duo-fpp", "Kiki_Main", 0, "Foo", "Bar");
        GetStreamsResponse response = new() { [Guid.NewGuid()] = [streamResponse] };

        _pubgReportApiMock.Setup(api => api.GetStreams(_accountId)).ReturnsAsync(response);

        List<StreamerShowdown> streamInfos = [];

        await _streamInfoService.GetStreamInfo(_accountId, Action);
        await _streamInfoService.GetStreamInfo(_accountId, Action);

        Assert.That(streamInfos, Has.Count.EqualTo(1));
        Assert.That(streamInfos[0].ShowdownTimeUtc.Value, Is.EqualTo(streamResponse.TimeEvent));

        return;

        void Action(IReadOnlyList<StreamerShowdown> list) => streamInfos.AddRange(list);
    }
}
