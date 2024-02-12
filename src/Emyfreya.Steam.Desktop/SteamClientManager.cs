namespace Emyfreya.Steam;

public sealed class SteamClientManager : ISteamClientManager
{
    private readonly Lazy<Result<ISteamClient>> _client;

    public SteamClientManager(Func<Result<ISteamClient>>? factory = null)
    {
        _client = new(factory ?? SteamClientFactory.BuildFromRegistry, isThreadSafe: false);
    }

    public void Dispose()
    {
        if (_client.IsValueCreated && _client.Value.IsSuccess)
        {
            _client.Value.Value.Dispose();
        }
    }

    public Result<bool> IsSubcribedApp(uint appId)
    {
        return _client
            .Value
            .Bind(steamClient => steamClient.SteamApps)
            .Bind<bool>(steamApps => steamApps.IsSubscribedApp(appId));
    }

    public Result<uint[]> GetInstalledApps()
    {
        return _client
            .Value
            .Bind(steamClient => steamClient.SteamAppList)
            .Bind<uint[]>(steamAppList => steamAppList.GetInstalledApps());
    }

    public Result<string> GetAppName(uint appId)
    {
        return _client
            .Value
            .Bind(steamClient => steamClient.SteamApps)
            .Bind(steamApps => steamApps.GetAppName(appId));
    }

    public Result<string> GetAppLogo(uint appId)
    {
        return _client
            .Value
            .Bind(steamClient => steamClient.SteamApps)
            .Bind(steamApps => steamApps.GetAppLogo(appId));
    }
}
