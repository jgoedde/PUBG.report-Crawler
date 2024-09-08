namespace PubgReportCrawler.ValueObjects;

using OneOf;

public sealed record GameMode(string Value)
{
    private static readonly List<string> ModeIdentifiers = ["solo", "duo", "squad"];

    /// <summary>
    /// Creates a new instance of type GameMode with a human-readable name.
    /// </summary>
    /// <returns>null if the <paramref name="codeName"/> is unknown, otherwise the GameMode.</returns>
    public static OneOf<GameMode, UnknownGameMode> CreateWithReadableName(string codeName)
    {
        GameMode? gameMode = (ModeIdentifiers.Where(codeName.Contains).Select(mode => new GameMode(mode))).FirstOrDefault();

        if (gameMode is null)
        {
            return new UnknownGameMode(codeName);
        }

        return gameMode;
    }

    public override string ToString()
    {
        return this.Value;
    }
}

public sealed record UnknownGameMode(string CodeName);
