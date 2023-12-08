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
            "MTE2MDYxNzQyOTE1MjU2NzU1OA.GwEoGg.mqlhkz6EB0ORf-9fN7RjTvIKL8R3zMkkKfsmyE");
        await discordClient.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await discordClient.StopAsync();
        await discordClient.LogoutAsync();
    }
}