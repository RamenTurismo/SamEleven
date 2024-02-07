namespace RamenTurismo.Steam;

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
        if (!NativeLibrary.TryLoad(dllPath, out nint handle))
        {
            return Result.Fail(new NativeLibraryLoadFailed(dllPath));
        }

        return SteamClient.Build(handle);
    }
}
