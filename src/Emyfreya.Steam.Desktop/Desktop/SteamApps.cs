namespace Emyfreya.Steam.Desktop;

internal sealed class SteamApps : ISteamApps
{
    private readonly VirtualClassWrapper<SteamApps001> _wrapper001;
    private readonly VirtualClassWrapper<SteamApps008> _wrapper008;
    private readonly StringBuilder _bindingStringBuilder;
    private readonly StringBuilder _valueStringBuilder;

    public SteamApps(VirtualClassWrapper<SteamApps001> wrapper001, VirtualClassWrapper<SteamApps008> wrapper008)
    {
        _wrapper001 = wrapper001;
        _wrapper008 = wrapper008;
        _bindingStringBuilder = new StringBuilder(capacity: 4, maxCapacity: 200);
        _valueStringBuilder = new StringBuilder(capacity: 24, maxCapacity: 200);
    }

    public bool IsSubscribedApp(uint appId)
    {
        return _wrapper008.GetDelegate<IsSubscribedApp>(v => v.IsSubscribedApp)(_wrapper008.InterfaceHandle, appId);
    }

    public Result<string> GetAppData(uint appId, string key)
    {
        const int valueLength = 1024;

        _bindingStringBuilder.Clear().Append(key);
        _valueStringBuilder.Clear();

        int result = _wrapper001.GetDelegate<GetAppData>(v => v.GetAppData)(_wrapper001.InterfaceHandle, appId, _bindingStringBuilder, _valueStringBuilder, valueLength);

        if (result <= 0) return Result.Fail(new AppDataNotFound(appId, key, result));

        return _valueStringBuilder.ToString();
    }

    public Result<string> GetAppName(uint appId) => GetAppData(appId, "name");

    public Result<string> GetAppLogo(uint appId) => GetAppData(appId, "logo");
}
