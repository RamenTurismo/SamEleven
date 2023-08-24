using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SamEleven.App.Abstractions;
using SamEleven.App.Steam.DesktopApi;
using SamEleven.App.Steam.WebApi;

namespace SamEleven.App.Steam;

internal sealed class SteamService : ISteamService
{
    private readonly SteamDesktopApiService _steamDesktopApiService;
    private readonly SteamCommunityWebApiService _steamCommunityWebApiService;
    private readonly ILogger _logger;

    public SteamService(
        SteamDesktopApiService steamDesktopApiService,
        SteamCommunityWebApiService steamCommunityWebApiService,
        ILogger<SteamService> logger)
    {
        _steamDesktopApiService = steamDesktopApiService;
        _steamCommunityWebApiService = steamCommunityWebApiService;
        _logger = logger;
    }

    public async ValueTask<IReadOnlyList<SteamGameInfo>> GetAllGamesAsync()
    {
        Task<IReadOnlyList<SteamGameInfo>> desktopApiResponse = Task.Run(_steamDesktopApiService.GetAllInstalledGames);
        Task<IReadOnlyList<SteamGameInfo>> webApiResponse = GetAllGamesFromWebApiAsync();

        IReadOnlyList<SteamGameInfo>[] results = await Task.WhenAll(desktopApiResponse, webApiResponse);

        return results
            .SelectMany(e => e)
            .GroupBy(e => e.Id, e => e, (k, v) => v.First())
            .ToArray();
    }

    private async Task<IReadOnlyList<SteamGameInfo>> GetAllGamesFromWebApiAsync()
    {
        ulong steamId64 = _steamDesktopApiService.GetSteamId();

        try
        {
            UserGamesResponse? response = await _steamCommunityWebApiService.GetAllSteamGamesAsync(steamId64);

            if (response == null)
            {
                return Array.Empty<SteamGameInfo>();
            }

            return response.Games
                .Select(e => new SteamGameInfo(
                    e.AppId,
                    e.Name.ToString(),
                    new Uri(e.Logo)))
                .ToArray();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Could not get games from steam community website");
            return Array.Empty<SteamGameInfo>();
        }
    }
}
