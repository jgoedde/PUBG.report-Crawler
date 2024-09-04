namespace PubgReportCrawler.Config;

using System.ComponentModel.DataAnnotations;

public sealed class AppSettings
{
    [Required(AllowEmptyStrings = false)]
    public string DiscordBotToken { get; init; } = null!;

    [Required(AllowEmptyStrings = false)]
    public string KraftonAccountId { get; init; } = null!;

    [Required]
    public ulong DiscordUserId { get; init; }
}

