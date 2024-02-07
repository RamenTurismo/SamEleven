namespace SamEleven.App.Steam;

internal sealed class SteamService : ISteamService
{
    private readonly SteamworksSdkApi _steamDesktopApiService;
    private readonly ISteamCommunityApi _steamCommunityApi;

    public SteamService(
        SteamworksSdkApi steamDesktopApiService,
        ISteamCommunityApi steamCommunityApi)
    {
        _steamDesktopApiService = steamDesktopApiService;
        _steamCommunityApi = steamCommunityApi;
    }

    public async ValueTask<SteamGameInfo[]> GetAllGamesAsync(CancellationToken cancellationToken = default)
    {
        ulong steamId64 = _steamDesktopApiService.GetSteamId();

        Task<IEnumerable<SteamGameInfo>> desktopApiResponse = Task.Run(_steamDesktopApiService.GetAllInstalledGames);
        Task<IEnumerable<SteamGameInfo>> webApiResponse = GetAllAppsFromCommunityAsync(steamId64, cancellationToken);

        IEnumerable<SteamGameInfo>[] results = await Task.WhenAll(desktopApiResponse, webApiResponse);

        return results
            .Aggregate((l, r) => l.Concat(r))
            .ToArray();
    }

    private async Task<IEnumerable<SteamGameInfo>> GetAllAppsFromCommunityAsync(ulong steamId, CancellationToken cancellationToken = default)
    {
        ApiResponse<UserGamesResponse> apiResponse = await _steamCommunityApi.GetAllGamesAsync(steamId, cancellationToken).ConfigureAwait(false);

        if (apiResponse.Content is null) return Enumerable.Empty<SteamGameInfo>();

        return apiResponse.Content.Games
            .Select(e => new SteamGameInfo(
                e.AppId,
                e.Name.ToString(),
                new Uri(e.Logo)))
            .ToArray();
    }
}
