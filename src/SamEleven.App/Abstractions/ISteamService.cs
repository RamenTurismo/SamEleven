using System.Collections.Generic;
using SamEleven.App.Steam;
using SamEleven.Steamworks;

namespace SamEleven.App.Abstractions;

public interface ISteamService
{
    void Initialize(SteamInstallationInfo installationInfo);

    IReadOnlyList<SteamGameInfo> GetAllInstalledGames();
}
