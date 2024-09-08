using System.Collections.Immutable;
using System.Globalization;
using OneOf;

namespace PubgReportCrawler.Models;

public sealed record GameMode
{
    public string Value { get; }

    private static readonly ImmutableList<string> ModeIdentifiers = ImmutableList.Create("solo", "duo", "squad");

    private GameMode(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new instance of type GameMode with a human-readable name.
    /// </summary>
    /// <param name="codeName">The code name to be validated.</param>
    /// <returns>A GameMode instance if the codeName matches, or UnknownGameMode if it doesn't.</returns>
    public static OneOf<GameMode, UnknownGameMode> CreateWithReadableName(string codeName)
    {
        var gameMode = ModeIdentifiers
            .FirstOrDefault(mode => codeName.Contains(mode, StringComparison.OrdinalIgnoreCase));

        if (gameMode is null)
        {
            return new UnknownGameMode(codeName);
        }

        return new GameMode(gameMode);
    }
}

public static class GameModeExtensions
{
    /// <summary>
    /// Converts the GameMode value to a capitalized string (e.g., "Solo" instead of "solo").
    /// </summary>
    /// <param name="mode">The GameMode instance to be formatted.</param>
    /// <returns>The capitalized string version of the GameMode.</returns>
    public static string ToCapitalizedString(this GameMode mode)
    {
        return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(mode.Value.ToLower());
    }
}

public sealed record UnknownGameMode(string CodeName);
