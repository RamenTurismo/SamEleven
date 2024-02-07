using System.Runtime.InteropServices;
using Gibbed.Steamworks.Interfaces;

namespace Gibbed.Steamworks.Wrappers;

public sealed class SteamAppList001 : NativeWrapper<ISteamAppList001>
{

    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    private delegate uint NativeGetNumInstalledApps(
        IntPtr self);
    [UnmanagedFunctionPointer(CallingConvention.ThisCall)]
    private delegate uint NativeGetInstalledApps(
        IntPtr self,
        ref uint[] appId,
        uint unMaxAppIDs);


    public uint GetNumInstalledApps()
    {
        return Call<uint, NativeGetNumInstalledApps>(Functions.GetNumInstalledApps, ObjectAddress);
    }
    public uint GetInstalledApps(
        ref uint[] appId,
        uint unMaxAppIDs)
    {
        return Call<uint, NativeGetNumInstalledApps>(Functions.GetNumInstalledApps, ObjectAddress, appId, unMaxAppIDs);
    }
}
