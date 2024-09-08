using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using PubgReportCrawler.Config;

namespace PubgReportCrawler.HostedServices;

// ReSharper disable once SuggestBaseTypeForParameterInConstructor
public class DiscordHostedService(DiscordSocketClient discordClient, IOptions<AppSettingsOptions> appSettings) : IHostedService
{
    private AppSettingsOptions AppSettings => appSettings.Value;

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

        Console.WriteLine(JsonConvert.SerializeObject(socketUser));
        // await socketUser.SendMessageAsync(message);
    }
}
