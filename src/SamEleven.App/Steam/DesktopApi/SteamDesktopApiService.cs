using System.Runtime.InteropServices;
using System.Text;
using ValveKeyValue;

namespace SamEleven.App.Steam.DesktopApi;

internal sealed class SteamDesktopApiService : IDisposable
{
    private bool _isInitiated = false;
    private readonly ILogger _logger;
    private readonly ISteamCdnService _steamCdnService;

    public SteamDesktopApiService(ILogger<SteamDesktopApiService> logger, ISteamCdnService steamCdnService)
    {
        _logger = logger;
        _steamCdnService = steamCdnService;
    }

    private void CheckIfInit()
    {
        if (_isInitiated) return;

        SteamAPI.Init();

        _isInitiated = true;
    }

    public void Dispose()
    {
        if (_isInitiated)
        {
            SteamAPI.Shutdown();
        }
    }

    public ulong GetSteamId()
    {
        CheckIfInit();

        return SteamUser.GetSteamID().m_SteamID;
    }

    public IReadOnlyList<SteamGameInfo> GetAllInstalledGames()
    {
        SteamInstallationInfo steamInstallationInfo = SteamInstallationInfo.FromRegistry();
        List<SteamGameInfo> games = new(capacity: steamInstallationInfo.AppsIds.Count);

        foreach (string item in steamInstallationInfo.AppsIds)
        {
            if (!uint.TryParse(item, out uint gameId))
            {
                _logger.LogInformation("{AppId} is not a valid steam app ID", item);

                continue;
            }

            // SteamAppData appdata = GetAppData(gameId);
            SteamAppData appdata = new("", "");
            if (appdata.Name == null)
            {
                continue;
            }

            games.Add(new SteamGameInfo(gameId, appdata.Name, appdata.Logo == null ? null : _steamCdnService.BuildGameImageUri(gameId, appdata.Logo)));
        }

        return games.AsReadOnly();
    }
}
