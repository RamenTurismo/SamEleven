namespace Emyfreya.Steam.Models;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct SteamAppList001
{
    public const string Name = "STEAMAPPLIST_INTERFACE_VERSION001";

    public nint GetNumInstalledApps;
    public nint GetInstalledApps;
}
