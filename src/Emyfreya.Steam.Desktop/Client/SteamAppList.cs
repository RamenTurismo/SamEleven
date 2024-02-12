namespace Emyfreya.Steam.Desktop.Client;

internal sealed class SteamAppList : ISteamAppList
{
    private readonly VirtualClassWrapper<SteamAppList001> _wrapper;

    public SteamAppList(VirtualClassWrapper<SteamAppList001> wrapper)
    {
        _wrapper = wrapper;
    }

    public uint GetNumInstalledApps()
    {
        return _wrapper.GetDelegate<GetNumInstalledApps>(v => v.GetNumInstalledApps)(_wrapper.InterfaceHandle);
    }

    public uint[] GetInstalledApps()
    {
        uint numInstalledApps = GetNumInstalledApps();
        uint[] apps = new uint[numInstalledApps];

        _wrapper.GetDelegate<GetInstalledApps>(v => v.GetInstalledApps)(_wrapper.InterfaceHandle, apps, numInstalledApps);

        return apps;
    }
}
