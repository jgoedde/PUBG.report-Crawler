using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using PubgReportCrawler.Config;

namespace PubgReportCrawler.HostedServices;

// ReSharper disable once SuggestBaseTypeForParameterInConstructor
public class DiscordHostedService(DiscordSocketClient discordClient, IOptions<AppOptions> appOptions) : IHostedService
{
    private AppOptions AppOptions => appOptions.Value;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await discordClient.LoginAsync(TokenType.Bot, AppOptions.DiscordBotToken);
        await discordClient.StartAsync();

        await Task.Delay(TimeSpan.FromSeconds(3), cancellationToken);

        await SendMessage("[PUBG.report Crawler] :arrow_up: Der Bot ist gestartet.");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await SendMessage("[PUBG.report Crawler] :arrow_down: Der Bot stoppt nun.");

        await discordClient.StopAsync();
        await discordClient.LogoutAsync();
    }

    private async Task SendMessage(string message)
    {
        var socketUser = await discordClient.GetUserAsync(AppOptions.DiscordUserId);
        if (socketUser is null)
        {
            return;
        }

        await socketUser.SendMessageAsync(message);
    }
}
