using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.Hosting;

using PubgReportCrawler.Entities;
using PubgReportCrawler.Services;
using PubgReportCrawler.ValueObjects;

namespace PubgReportCrawler.HostedServices;

public class PubgReportHostedService(
    StreamInfoService streamInfoService,
    DiscordSocketClient discordSocketClient) : IHostedService, IDisposable
{
    private Timer? _timer;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(30));

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

    private async void DoWork(object? state)
    {
        await streamInfoService.GetStreamInfo(new PubgReportAccountId("account.1eb237a0e35b45d7a041b76681851604"),
            OnNewStreamInfo);
    }

    private async void OnNewStreamInfo(IReadOnlyList<StreamInfo> info)
    {
        IUser? socketUser = await discordSocketClient.GetUserAsync(281128906243702784);
        if (socketUser is null)
        {
            return;
        }

        await socketUser.SendMessageAsync(
            $"{info.Count} neue Streamer-Interaktionen: https://pubg.report/players/account.1eb237a0e35b45d7a041b76681851604");
    }
}