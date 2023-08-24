using System;
using System.Collections.Generic;
using Gibbed.Steamworks;
using Microsoft.Extensions.Logging;
using SamEleven.App.Abstractions;
using SamEleven.Steamworks;

namespace SamEleven.App.Steam.DesktopApi;

internal sealed class SteamDesktopApiService : IDisposable
{
    private ValueTuple<uint, GibbedSteamClient>? _activeClientCache;
    private readonly ILogger _logger;
    private readonly ISteamCdnService _steamCdnService;

    public SteamDesktopApiService(ILogger<SteamDesktopApiService> logger, ISteamCdnService steamCdnService)
    {
        _logger = logger;
        _steamCdnService = steamCdnService;
    }

    private GibbedSteamClient GetOrCreateClient(uint id)
    {
        if (_activeClientCache?.Item1 == id) return _activeClientCache.Value.Item2;

        _activeClientCache?.Item2.Dispose();

        GibbedSteamClient client = new();
        client.Initialize(id);

        _activeClientCache = ValueTuple.Create(id, client);

        return client;
    }

    public void Dispose()
    {
        _activeClientCache?.Item2.Dispose();
    }

    public SteamAppData GetAppData(uint appId)
    {
        GibbedSteamClient gibbedSteamClient = GetOrCreateClient(0);
        string? name = gibbedSteamClient.SteamApps001!.GetAppData(appId, SteamAppData.NameKey);
        string? logo = gibbedSteamClient.SteamApps001!.GetAppData(appId, SteamAppData.LogoKey);

        return new SteamAppData(name, logo);
    }

    public ulong GetSteamId()
    {
        GibbedSteamClient gibbedSteamClient = GetOrCreateClient(0);
        return gibbedSteamClient.SteamUser!.GetSteamId();
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

            SteamAppData appdata = GetAppData(gameId);
            if (appdata.Name == null)
            {
                continue;
            }

            games.Add(new SteamGameInfo(gameId, appdata.Name, appdata.Logo == null ? null : _steamCdnService.BuildGameImageUri(gameId, appdata.Logo)));
        }

        return games.AsReadOnly();
    }
}
