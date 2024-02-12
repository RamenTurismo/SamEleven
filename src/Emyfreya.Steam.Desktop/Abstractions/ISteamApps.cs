namespace Emyfreya.Steam.Abstractions;

public interface ISteamApps
{
    string GetAppData(uint appId, string key);
    string GetAppLogo(uint appId);
    string GetAppName(uint appId);
    bool IsSubscribedApp(uint appId);
}
