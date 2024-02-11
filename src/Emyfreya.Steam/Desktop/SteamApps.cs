namespace Emyfreya.Steam.Desktop;

internal sealed class SteamApps
{
    private readonly VirtualClassWrapper<SteamApps001> _wrapper;
    private readonly StringBuilder _bindingStringBuilder;
    private readonly StringBuilder _valueStringBuilder;

    public SteamApps(VirtualClassWrapper<SteamApps001> wrapper)
    {
        _wrapper = wrapper;
        _bindingStringBuilder = new StringBuilder(capacity: 4, maxCapacity: 10);
        _valueStringBuilder = new StringBuilder(capacity: 24, maxCapacity: 200);
    }

    public string GetAppData(uint appId, string key)
    {
        const int valueLength = 1024;

        _bindingStringBuilder.Clear().Append(key);
        _valueStringBuilder.Clear();

        int result = _wrapper.GetDelegate<GetAppData>(v => v.GetAppData)(_wrapper.InterfaceHandle, appId, _bindingStringBuilder, _valueStringBuilder, valueLength);

        return _valueStringBuilder.ToString();
    }

    public string GetAppName(uint appId) => GetAppData(appId, "name");

    public string GetAppLogo(uint appId) => GetAppData(appId, "logo");
}
