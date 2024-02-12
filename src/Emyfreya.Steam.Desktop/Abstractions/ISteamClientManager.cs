namespace Emyfreya.Steam.Abstractions;

public interface ISteamClientManager : IDisposable
{
    Result<bool> IsSubcribedApp(uint appId);
    Result<string> GetAppName(uint appId);
    Result<string> GetAppLogo(uint appId);
    Result<uint[]> GetInstalledApps();
}
