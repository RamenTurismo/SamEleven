namespace Emyfreya.Steam.Abstractions;

public interface ISteamService
{
    Result<string> GetAppName(uint appId);
    Result<string> GetAppLogo(uint appId);
}
