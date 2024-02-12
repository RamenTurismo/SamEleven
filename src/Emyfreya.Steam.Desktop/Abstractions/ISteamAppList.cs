namespace Emyfreya.Steam.Desktop.Abstractions;

public interface ISteamAppList
{
    uint[] GetInstalledApps();
    uint GetNumInstalledApps();
}
