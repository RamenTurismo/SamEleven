namespace RamenTurismo.Steam.Models;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct SteamApps001
{
    public const string Name = "STEAMAPPS_INTERFACE_VERSION001";

    public IntPtr GetAppData;
}
