namespace Emyfreya.Steam.Desktop;

internal static class SteamClientFactory
{
    public static Result<SteamClient> BuildFromRegistry()
    {
        Result<SteamInstallationInfo> installationInfo = SteamInstallationInfoFactory.FromRegistry();

        if (installationInfo.IsFailed) return installationInfo.ToResult<SteamClient>();

        string path = Path.Combine(installationInfo.Value.InstallPath, SteamConsts.SteamClientDllName);

        return BuildFromPath(path);
    }

    public static Result<SteamClient> BuildFromPath(string dllPath)
    {
        return Result
            .Try(() => NativeLibrary.Load(dllPath), ex => new NativeLibraryLoadFailed(dllPath, ex))
            .Bind(SteamClient.Build);
    }
}
