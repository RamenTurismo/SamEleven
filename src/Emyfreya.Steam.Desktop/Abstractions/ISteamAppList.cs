namespace Emyfreya.Steam.Abstractions;

public interface ISteamAppList
{
    uint[] GetInstalledApps();
    uint GetNumInstalledApps();
}
