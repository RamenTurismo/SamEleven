namespace Emyfreya.Steam.Abstractions;

public interface ISteamApps
{
    Result<string> GetAppData(uint appId, string key);
    Result<string> GetAppLogo(uint appId);
    Result<string> GetAppName(uint appId);
    bool IsSubscribedApp(uint appId);
}
