using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PubgReportCrawler.Config;
using PubgReportCrawler.Entities;
using PubgReportCrawler.Services;
using PubgReportCrawler.ValueObjects;

namespace PubgReportCrawler.HostedServices;

public sealed class PubgReportHostedService : IHostedService, IDisposable
{
    private Timer? _timer;
    private const int EveryXMinutes = 30;
    private bool _isReady;

    private readonly StreamInfoService _streamInfoService;
    private readonly DiscordSocketClient _discordSocketClient;
    private readonly AppSettingsOptions _appSettings;

    public PubgReportHostedService(StreamInfoService streamInfoService,
        DiscordSocketClient discordSocketClient, IOptions<AppSettingsOptions> appSettings)
    {
        _streamInfoService = streamInfoService;
        _discordSocketClient = discordSocketClient;
        _appSettings = appSettings.Value;

        discordSocketClient.Ready += () =>
        {
            _isReady = true;
            return Task.CompletedTask;
        };
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(EveryXMinutes));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose() => _timer?.Dispose();

    private async void DoWork(object? state)
    {
        if (!_isReady)
        {
            _timer?.Change(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(10));
            return;
        }

        _timer?.Change(TimeSpan.FromMinutes(EveryXMinutes), TimeSpan.FromMinutes(EveryXMinutes));

        await _streamInfoService.GetStreamInfo(new PubgReportAccountId(_appSettings.KraftonAccountId),
            OnNewStreamInfo);
    }

    private async void OnNewStreamInfo(IReadOnlyList<StreamerShowdown> infos)
    {
        if (true)
        {
            return;
        }

        var socketUser = await _discordSocketClient.GetUserAsync(_appSettings.DiscordUserId);
        if (socketUser is null)
        {
            return;
        }

        var sendMessageTaskList = new List<Task>();

        sendMessageTaskList.AddRange(infos.Select(streamInfo =>
        {
            GameMode gameMode = streamInfo.MatchDetails.GameMode;
            Map map = streamInfo.MatchDetails.Map;
            return socketUser.SendMessageAsync(string.Format(
                $"Du bist im {gameMode} einem Streamer begegnet auf {map}. Mehr Infos: https://pubg.report/players/{_appSettings.KraftonAccountId}"));
        }));

        await Task.WhenAll(sendMessageTaskList);
    }
}
