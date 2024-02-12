namespace Emyfreya.Steam.Desktop.Client;

internal static class SteamClientFactory
{
    public static Result<ISteamClient> BuildFromRegistry()
    {
        Result<SteamInstallationInfo> installationInfo = SteamInstallationInfoFactory.FromRegistry();

        if (installationInfo.IsFailed) return installationInfo.ToResult<ISteamClient>();

        string path = Path.Combine(installationInfo.Value.InstallPath, SteamConsts.SteamClientDllName);

        return BuildFromPath(path);
    }

    public static Result<ISteamClient> BuildFromPath(string dllPath)
    {
        return Result
            .Try(() => NativeLibrary.Load(dllPath), ex => new NativeLibraryLoadFailed(dllPath, ex))
            .Bind(SteamClient.Build);
    }
}
