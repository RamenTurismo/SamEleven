namespace SamEleven.App.Steam.Models;

public sealed record SteamGameInfo(
    uint Id,
    string Name,
    Uri? Image);
