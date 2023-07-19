namespace SamEleven.App.Steam;

internal sealed class SteamCdnOptions
{
    public const string SectionName = "SteamCdn";

    public string GameImageUrl { get; init; } = null!;
}
