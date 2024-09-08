using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PubgReportCrawler.Config;
using PubgReportCrawler.Models;
using PubgReportCrawler.Services;

namespace PubgReportCrawler.HostedServices;

public sealed class PubgReportHostedService(
    StreamInfoService streamInfoService,
    DiscordSocketClient discordSocketClient,
    IOptions<AppOptions> appOptions,
    ShowdownReportProvider showdownReportProvider)
    : BackgroundService
{
    private readonly TimeSpan _period = TimeSpan.FromSeconds(30);

    private readonly AppOptions _appOptions = appOptions.Value;

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new(_period);

        while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
        {
            await streamInfoService.GetStreamInfo(new PubgReportAccountId(_appOptions.KraftonAccountId),
                OnNewStreamInfo);
        }
    }

    private async void OnNewStreamInfo(IReadOnlyList<StreamerShowdown> infos)
    {
        var socketUser = await discordSocketClient.GetUserAsync(_appOptions.DiscordUserId);
        if (socketUser is null)
        {
            return;
        }

        // We do not want to spam the user with each showdown report per message.
        // This is happening when the app is restarted and so the cache containing the past streamer showdowns is cleared.
        if (infos.Count >= 3)
        {
            await socketUser.SendMessageAsync(showdownReportProvider.GetManyMoreShowdownsMessage());
            return;
        }

        var sendMessageTaskList = new List<Task>();

        sendMessageTaskList.AddRange(infos.Select(showdown
            => socketUser.SendMessageAsync(showdownReportProvider.GetMessage(showdown))));

        await Task.WhenAll(sendMessageTaskList);
    }
}
