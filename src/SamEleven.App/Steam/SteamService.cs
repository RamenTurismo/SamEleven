namespace SamEleven.App.Steam;

internal sealed partial class SteamService : ISteamService
{
    private static partial class Log
    {
        [LoggerMessage(LogLevel.Debug, "Fetching subscribed AppIds from AppIds of length {Length}.")]
        public static partial void FetchingAppIds(ILogger logger, int length);
        [LoggerMessage(LogLevel.Information, "Found {Subscribed} subscribed AppIds in {Elapsed}ms from total of {Length} AppIds.")]
        public static partial void FoundSubscribedAppIds(ILogger logger, ushort subscribed, long elapsed, int length);
        [LoggerMessage(LogLevel.Information, "Found {Subscribed} subscribed AppIds in cache.")]
        public static partial void FoundSubscribedAppIdsCache(ILogger logger, int subscribed);
        [LoggerMessage(LogLevel.Trace, "User not subscribed to app '{AppId}'. Error={Error}")]
        public static partial void IsSubcribedAppFailed(ILogger logger, uint appId, IError? error);
    }

    private readonly ISteamClientManager _steamDesktop;
    private readonly ISteamApi _steamWebApi;
    private readonly ILogger _logger;
    private readonly IFileCacheService _fileCacheService;

    public SteamService(
        ISteamApi steamWebApi,
        ISteamClientManager steamDesktop,
        ILogger<SteamService> logger,
        IFileCacheService fileCacheService)
    {
        _steamWebApi = steamWebApi;
        _steamDesktop = steamDesktop;
        _logger = logger;
        _fileCacheService = fileCacheService;
    }

    public async IAsyncEnumerable<SteamApp> GetAllGamesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        IReadOnlyList<SteamApp> cached = await GetAllGamesFromCacheAsync(cancellationToken).ConfigureAwait(false);

        if (cached.Count > 0)
        {
            foreach (SteamApp app in cached)
            {
                yield return app;
            }
        }
        else
        {
            List<SteamApp> apps = new(capacity: 100);

            await foreach (SteamApp app in GetAllGamesFromWebApiAsync(cancellationToken))
            {
                apps.Add(app);
                yield return app;
            }

            await _fileCacheService.SaveAsync("apps.json", apps, cancellationToken).ConfigureAwait(false);
        }
    }

    private async Task<IReadOnlyList<SteamApp>> GetAllGamesFromCacheAsync(CancellationToken cancellationToken = default)
    {
        List<SteamApp>? cached = await _fileCacheService.GetAsync<List<SteamApp>>("apps.json", cancellationToken).ConfigureAwait(false);

        if (cached is null)
        {
            return Array.Empty<SteamApp>();
        }

        Log.FoundSubscribedAppIdsCache(_logger, cached.Count);

        return cached.AsReadOnly();
    }

    private async IAsyncEnumerable<SteamApp> GetAllGamesFromWebApiAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using ApiResponse<GetAppListResult> appList = await _steamWebApi.GetAppListAsync(cancellationToken: cancellationToken).ConfigureAwait(false);

        if (!appList.IsSuccessStatusCode) yield break;

        GetAppListAppResult[] apps = appList.Content.AppList.Apps;

        Log.FetchingAppIds(_logger, apps.Length);
        Stopwatch stopwatch = Stopwatch.StartNew();
        ushort subscribed = 0;

        foreach (GetAppListAppResult app in apps)
        {
            Result<bool> isSubcribed = _steamDesktop.IsSubcribedApp(app.AppId);

            if (isSubcribed.IsFailed || !isSubcribed.Value)
            {
                Log.IsSubcribedAppFailed(_logger, app.AppId, isSubcribed.Errors.FirstOrDefault());
                continue;
            }

            yield return new SteamApp(app.AppId, app.Name, BuildLogoUri(app.AppId));

            subscribed++;
        }

        Log.FoundSubscribedAppIds(_logger, subscribed, stopwatch.ElapsedMilliseconds, apps.Length);
    }

    private Uri? BuildLogoUri(uint appId)
    {
        return new Uri($"https://cdn.cloudflare.steamstatic.com/steam/apps/{appId}/capsule_231x87.jpg");
    }
}
