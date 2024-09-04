namespace PubgReportCrawler.HostedServices;

using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PubgReportCrawler.Config;
using PubgReportCrawler.Entities;
using PubgReportCrawler.Services;
using PubgReportCrawler.ValueObjects;

public sealed class PubgReportHostedService : IHostedService, IDisposable
{
    private Timer? _timer;
    private const int EveryXMinutes = 30;
    private bool _isReady;

    private readonly StreamInfoService _streamInfoService;
    private readonly DiscordSocketClient _discordSocketClient;
    private readonly AppSettings _appSettings;

    public PubgReportHostedService(StreamInfoService streamInfoService,
        DiscordSocketClient discordSocketClient, IOptions<AppSettings> appSettings)
    {
        this._streamInfoService = streamInfoService;
        this._discordSocketClient = discordSocketClient;
        this._appSettings = appSettings.Value;

        discordSocketClient.Ready += () =>
        {
            this._isReady = true;
            return Task.CompletedTask;
        };
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        this._timer = new Timer(this.DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(EveryXMinutes));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        this._timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose() => this._timer?.Dispose();

    private async void DoWork(object? state)
    {
        if (!this._isReady)
        {
            this._timer?.Change(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(10));
            return;
        }

        this._timer?.Change(TimeSpan.FromMinutes(EveryXMinutes), TimeSpan.FromMinutes(EveryXMinutes));

        await this._streamInfoService.GetStreamInfo(new PubgReportAccountId(_appSettings.KraftonAccountId),
            this.OnNewStreamInfo);
    }

    private async void OnNewStreamInfo(IReadOnlyList<StreamInfo> info)
    {
        if (IsDevelopment())
        {
            return;
        }

        var socketUser = await this._discordSocketClient.GetUserAsync(_appSettings.DiscordUserId);
        if (socketUser is null)
        {
            return;
        }

        // TODO: Include streamer's name on twitch. Possibly the viewer count?
        await socketUser.SendMessageAsync(
            $"{info.Count} neue Streamer-Interaktionen: https://pubg.report/players/{_appSettings.KraftonAccountId}");
    }

    private static bool IsDevelopment() => Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
}
