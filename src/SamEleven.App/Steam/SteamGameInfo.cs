using System;

namespace SamEleven.App.Steam;

public sealed record SteamGameInfo(
    uint Id,
    string Name,
    Uri? Image);
