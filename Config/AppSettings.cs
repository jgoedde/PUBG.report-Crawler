namespace PubgReportCrawler.Config;

using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.Options;

public sealed class AppSettingsOptions
{
    [Required(AllowEmptyStrings = false)]
    public string DiscordBotToken { get; init; } = null!;

    [Required(AllowEmptyStrings = false)]
    public string KraftonAccountId { get; init; } = null!;

    [Required]
    public ulong DiscordUserId { get; init; }
}

public sealed class AppSettingsValidator : IValidateOptions<AppSettingsOptions>
{
    public ValidateOptionsResult Validate(string? name, AppSettingsOptions options)
    {
        if (string.IsNullOrWhiteSpace(options.DiscordBotToken))
        {
            return ValidateOptionsResult.Fail("foobar");
        }

        return ValidateOptionsResult.Success;
    }
}
