namespace PubgReportCrawler.HostedServices;

using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PubgReportCrawler.Config;

// ReSharper disable once SuggestBaseTypeForParameterInConstructor
public class DiscordHostedService(DiscordSocketClient discordClient, IOptions<AppSettings> appSettings) : IHostedService
{
    private AppSettings AppSettings => appSettings.Value;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await discordClient.LoginAsync(TokenType.Bot, AppSettings.DiscordBotToken);
        await discordClient.StartAsync();

        await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);

        await SendMessage(AppSettings.DiscordUserId, "[PUBG.report Crawler] [Status] Der Bot ist gestartet.");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await SendMessage(AppSettings.DiscordUserId, "[PUBG.report Crawler] [Status] Der Bot stoppt nun.");

        await discordClient.StopAsync();
        await discordClient.LogoutAsync();
    }

    private async Task SendMessage(ulong discordUserId, string message)
    {
        var socketUser = await discordClient.GetUserAsync(AppSettings.DiscordUserId);
        if (socketUser is null)
        {
            return;
        }

        await socketUser.SendMessageAsync(message);
    }
}
