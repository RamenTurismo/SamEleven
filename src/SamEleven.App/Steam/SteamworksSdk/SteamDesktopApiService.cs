using Gibbed.Steamworks;

namespace SamEleven.App.Steam.DesktopApi;

internal sealed partial class SteamworksSdkApi : IDisposable
{
    internal static partial class Log
    {
        [LoggerMessage(LogLevel.Debug, "Getting installed apps")]
        public static partial void GettingInstalledApps(ILogger logger);
        [LoggerMessage(LogLevel.Information, "Got {length} installed apps")]
        public static partial void GotInstalledApps(ILogger logger, uint length);
    }

    private readonly ILogger _logger;
    private readonly ISteamCdnService _steamCdnService;
    private readonly Lazy<GibbedSteamClient> _client;

    public SteamworksSdkApi(ILogger<SteamworksSdkApi> logger, ISteamCdnService steamCdnService)
    {
        _logger = logger;
        _steamCdnService = steamCdnService;
        _client = new Lazy<GibbedSteamClient>(CreateClient, isThreadSafe: false);
    }

    public void Dispose()
    {
        if (_client.IsValueCreated)
        {
            _client.Value.Dispose();
        }
    }

    public ulong GetSteamId()
    {
        return _client.Value.SteamUser!.GetSteamId();
    }

    public uint[] GetInstalledAppsIds()
    {
        Log.GettingInstalledApps(_logger);

        uint numApps = _client.Value.SteamAppList001!.GetNumInstalledApps();
        uint[] ids = new uint[numApps];
        uint length = _client.Value.SteamAppList001!.GetInstalledApps(ref ids, numApps);

        Log.GotInstalledApps(_logger, length);

        return ids;
    }

    public IEnumerable<SteamGameInfo> GetAllInstalledGames()
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

            Uri? logo = appdata.Logo == null ? null : _steamCdnService.BuildGameImageUri(gameId, appdata.Logo);

            games.Add(new SteamGameInfo(
                gameId,
                appdata.Name,
               logo));
        }

        return games.AsReadOnly();
    }

    public SteamAppData GetAppData(uint appId)
    {
        string? name = _client.Value.SteamApps001!.GetAppData(appId, SteamAppData.NameKey);
        string? logo = _client.Value.SteamApps001!.GetAppData(appId, SteamAppData.LogoKey);

        return new SteamAppData(name, logo);
    }

    private GibbedSteamClient CreateClient()
    {
        // NativeLibrary.SetDllImportResolver(Assembly.GetExecutingAssembly(), DllImportResolver);

        GibbedSteamClient client = new();
        client.Initialize(0);

        return client;
    }

    private static IntPtr DllImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        if (libraryName == "steam_api64")
        {
            return NativeLibrary.Load(Path.Combine(SteamInstallationInfo.FromRegistry().InstallPath, "steamclient64.dll"), assembly, searchPath);
        }

        if (libraryName == "steam_api")
        {
            return NativeLibrary.Load(Path.Combine(SteamInstallationInfo.FromRegistry().InstallPath, "steamclient.dll"), assembly, searchPath);
        }

        // Otherwise, fallback to default import resolver.
        return IntPtr.Zero;
    }
}
