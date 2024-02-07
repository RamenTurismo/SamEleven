using System.Runtime.InteropServices;

namespace Gibbed.Steamworks.Interfaces;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct ISteamAppList001
{
    public IntPtr GetNumInstalledApps;
    public IntPtr GetInstalledApps;
}
