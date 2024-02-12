namespace Emyfreya.Steam.Abstractions;

public interface ISteamClient : IDisposable
{
    Result<ISteamAppList> SteamAppList { get; }
    Result<ISteamApps> SteamApps { get; }
    void Dispose();
}
