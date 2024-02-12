namespace SamEleven.App.Steam;

internal sealed partial class SteamService : ISteamService
{
    private static partial class Log
    {
        [LoggerMessage(LogLevel.Debug, "Fetching subscribed AppIds from AppIds of length {Length}.")]
        public static partial void FetchingAppIds(ILogger logger, int length);
        [LoggerMessage(LogLevel.Information, "Found {Subscribed} subscribed AppIds in {Elapsed}ms from total of {Length} AppIds.")]
        public static partial void FoundSubscribedAppIds(ILogger logger, ushort subscribed, long elapsed, int length);
    }

    private readonly ISteamClientManager _steamDesktop;
    private readonly ISteamApi _steamWebApi;
    private readonly ILogger _logger;

    public SteamService(ISteamApi steamWebApi, ISteamClientManager steamDesktop, ILogger<SteamService> logger)
    {
        _steamWebApi = steamWebApi;
        _steamDesktop = steamDesktop;
        _logger = logger;
    }

    public async IAsyncEnumerable<SteamGameInfo> GetAllGamesAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using ApiResponse<GetAppListResult> appList = await _steamWebApi.GetAppListAsync(cancellationToken: cancellationToken);

        if (!appList.IsSuccessStatusCode) yield break;

        GetAppListAppResult[] apps = appList.Content.AppList.Apps;

        Log.FetchingAppIds(_logger, apps.Length);
        Stopwatch stopwatch = Stopwatch.StartNew();
        ushort subscribed = 0;

        foreach (GetAppListAppResult app in apps)
        {
            Result<bool> isSubcribed = _steamDesktop.IsSubcribedApp(app.AppId);

            if (isSubcribed.IsFailed || !isSubcribed.Value) continue;

            yield return new SteamGameInfo(app.AppId, app.Name, BuildLogoUri(app.AppId));

            subscribed++;
        }

        Log.FoundSubscribedAppIds(_logger, subscribed, stopwatch.ElapsedMilliseconds, apps.Length);
    }

    private Uri? BuildLogoUri(uint appId)
    {
        return new Uri($"https://cdn.cloudflare.steamstatic.com/steam/apps/{appId}/capsule_231x87.jpg");
    }
}
