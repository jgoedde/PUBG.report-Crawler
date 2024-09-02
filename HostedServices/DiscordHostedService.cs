using Discord;
using Discord.WebSocket;

using Microsoft.Extensions.Hosting;

namespace PubgReportCrawler.HostedServices;

// ReSharper disable once SuggestBaseTypeForParameterInConstructor
public class DiscordHostedService(DiscordSocketClient discordClient) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await discordClient.LoginAsync(TokenType.Bot,
            "MTE2MDYxNzQyOTE1MjU2NzU1OA.GnYWK7.cm-GzzfoMC4iPF-uxde7rYKKP3PWzOc2TQB8TA");
        await discordClient.StartAsync();

        IUser? socketUser = await discordClient.GetUserAsync(281128906243702784);
        if (socketUser is null)
        {
            return;
        }

        await socketUser.SendMessageAsync("[PUBG.report Crawler] [Status] Der Discord-Bot ist gestartet.");
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        IUser? socketUser = await discordClient.GetUserAsync(281128906243702784);
        if (socketUser is not null)
        {
            await socketUser.SendMessageAsync("[PUBG.report Crawler] [Status] Der Bot stoppt nun.");
        }

        await discordClient.StopAsync();
        await discordClient.LogoutAsync();
    }
}
