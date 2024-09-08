namespace PubgReportCrawler.Config;

using System.ComponentModel.DataAnnotations;

public sealed record AppSettingsOptions(
    [Required(AllowEmptyStrings = false)]
    string DiscordBotToken,

    [Required(AllowEmptyStrings = false)]
    string KraftonAccountId,

    [Required]
    ulong DiscordUserId,

    [Required(AllowEmptyStrings = false)]
    string PubgNick
);
