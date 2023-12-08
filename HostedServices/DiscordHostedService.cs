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
            "MTE2MDYxNzQyOTE1MjU2NzU1OA.G0zET0.Gh59oX-eO07kmpgolOzKD7dR23S_qb83Iho3XU");
        await discordClient.StartAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await discordClient.StopAsync();
        await discordClient.LogoutAsync();
    }
}