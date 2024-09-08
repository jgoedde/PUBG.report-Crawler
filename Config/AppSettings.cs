using Microsoft.Extensions.Configuration;
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace PubgReportCrawler.Config;

using System.ComponentModel.DataAnnotations;

public sealed class AppOptions
{
    [ConfigurationKeyName("DISCORD_BOT_TOKEN")]
    [Required(AllowEmptyStrings = false)]
    public string DiscordBotToken { get; init; } = null!;

    [ConfigurationKeyName("KRAFTON_ACCOUNT_ID")]
    [Required(AllowEmptyStrings = false)]
    public string KraftonAccountId { get; init; } = null!;

    [ConfigurationKeyName("DISCORD_USER_ID")]
    [Required]
    public ulong DiscordUserId { get; init; }

    [ConfigurationKeyName("PUBG_NICK")]
    [Required(AllowEmptyStrings = false)]
    public string PubgNick { get; init; } = null!;
}
