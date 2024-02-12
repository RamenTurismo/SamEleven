namespace SamEleven.App.Steam.Models;

public sealed record SteamApp(
    uint Id,
    string Name,
    Uri? Image);
