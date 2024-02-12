namespace Emyfreya.Steam.Desktop.Models;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct SteamApps001
{
    public const string Name = "STEAMAPPS_INTERFACE_VERSION001";

    public nint GetAppData;
}
