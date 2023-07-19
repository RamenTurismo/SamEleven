using SamEleven.App.Steam.Client;

namespace SamEleven.App.Abstractions;

public interface ISteamClient
{
    SteamAppData GetAppData(uint appId);
}
