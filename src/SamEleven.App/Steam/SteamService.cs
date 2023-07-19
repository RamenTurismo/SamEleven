using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using SamEleven.App.Abstractions;
using SamEleven.App.Steam.Client;
using SamEleven.Steamworks;

namespace SamEleven.App.Steam;

internal sealed class SteamService : ISteamService
{
    private readonly ISteamClient _steamClient;
    private readonly ISteamCdnService _steamCdnService;
    private readonly ILogger _logger;

    private SteamInstallationInfo _steamInstallationInfo = null!;

    public SteamService(
        ISteamClient steamClient,
        ISteamCdnService steamCdnService,
        ILogger<SteamService> logger)
    {
        _steamClient = steamClient;
        _steamCdnService = steamCdnService;
        _logger = logger;
    }

    public void Initialize(SteamInstallationInfo installationInfo)
    {
        _steamInstallationInfo = installationInfo;
    }

    public IReadOnlyList<SteamGameInfo> GetAllInstalledGames()
    {
        List<SteamGameInfo> games = new(capacity: _steamInstallationInfo.AppsIds.Count);

        foreach (string item in _steamInstallationInfo.AppsIds)
        {
            if (!uint.TryParse(item, out uint gameId))
            {
                _logger.LogInformation("{AppId} is not a valid steam app ID", item);

                continue;
            }

            SteamAppData appdata = _steamClient.GetAppData(gameId);

            if (appdata.Name == null)
            {
                continue;
            }

            games.Add(new SteamGameInfo(gameId, appdata.Name, appdata.Logo == null ? null : _steamCdnService.BuildGameImageUri(gameId, appdata.Logo)));
        }

        return games.AsReadOnly();
    }
}
