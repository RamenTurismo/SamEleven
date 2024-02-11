namespace Emyfreya.Steam;

internal sealed class SteamService : ISteamService
{
    private readonly Lazy<Result<SteamClient>> _client;

    public SteamService()
    {
        _client = new Lazy<Result<SteamClient>>(SteamClientFactory.BuildFromRegistry, isThreadSafe: false);
    }

    public Result<string> GetAppName(uint appId)
    {
        return _client
            .Value
            .Bind(steamClient => steamClient.SteamApps)
            .Bind<string>(steamApps => steamApps.GetAppName(appId));
    }

    public Result<string> GetAppLogo(uint appId)
    {
        return _client
            .Value
            .Bind(steamClient => steamClient.SteamApps)
            .Bind<string>(steamApps => steamApps.GetAppLogo(appId));
    }
}
