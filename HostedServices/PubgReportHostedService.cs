using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.Hosting;

using PubgReportCrawler.Entities;
using PubgReportCrawler.Services;
using PubgReportCrawler.ValueObjects;

namespace PubgReportCrawler.HostedServices;

public class PubgReportHostedService : IHostedService, IDisposable
{
    private Timer? _timer;
    private const int EveryXMinutes = 30;
    private bool _isReady;

    private readonly StreamInfoService _streamInfoService;
    private readonly DiscordSocketClient _discordSocketClient;

    public PubgReportHostedService(StreamInfoService streamInfoService,
        DiscordSocketClient discordSocketClient)
    {
        _streamInfoService = streamInfoService;
        _discordSocketClient = discordSocketClient;

        discordSocketClient.Ready += () =>
        {
            _isReady = true;
            return Task.CompletedTask;
        };
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWorkWrapper, null, TimeSpan.Zero, TimeSpan.FromMinutes(EveryXMinutes));

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Change(Timeout.Infinite, 0);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _timer?.Dispose();
    }

    private async void DoWorkWrapper(object? state) {
        try {
            DoWork(state);
        } catch(Exception e) {
            Console.WriteLine(e);
        }
    }

    private async void DoWork(object? state)
    {
        if (!_isReady)
        {
            _timer?.Change(TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(10));
            return;
        }

        _timer?.Change(TimeSpan.FromMinutes(EveryXMinutes), TimeSpan.FromMinutes(EveryXMinutes));

        await _streamInfoService.GetStreamInfo(new PubgReportAccountId("account.1eb237a0e35b45d7a041b76681851604"),
            OnNewStreamInfo);
    }

    private async void OnNewStreamInfo(IReadOnlyList<StreamInfo> info)
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            return;
        }

        IUser? socketUser = await _discordSocketClient.GetUserAsync(281128906243702784);
        if (socketUser is null)
        {
            return;
        }

        await socketUser.SendMessageAsync(
            $"{info.Count} neue Streamer-Interaktionen: https://pubg.report/players/account.1eb237a0e35b45d7a041b76681851604");
    }
}
