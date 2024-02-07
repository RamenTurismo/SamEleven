namespace RamenTurismo.Steam;

public static class SteamInstallationInfoFactory
{
    public static Result<SteamInstallationInfo> FromRegistry()
    {
        const RegistryHive registryHive = RegistryHive.LocalMachine;
        const RegistryView registryView = RegistryView.Registry32;

        // Gotta make it work for x86+x64.
        //
        // View keys :
        // https://learn.microsoft.com/en-us/troubleshoot/windows-client/deployment/view-system-registry-with-64-bit-windows#view-64-bit-and-32-bit-registry-keys
        using RegistryKey? localKey = RegistryKey.OpenBaseKey(registryHive, registryView);
        if (localKey is null) return Result.Fail(new RegistryBaseKeyNotFound(registryHive, registryView));

        using RegistryKey? steamKey = localKey.OpenSubKey(SteamConsts.RegistryPath, false);
        if (steamKey is null) return Result.Fail(new RegistrySubKeyNotFound(localKey, SteamConsts.RegistryPath));

        using RegistryKey? apps = localKey.OpenSubKey(SteamConsts.RegistryAppsKey, false);
        if (apps is null) return Result.Fail(new RegistrySubKeyNotFound(steamKey, SteamConsts.RegistryAppsKey));

        string? installPath = steamKey.GetValueToString(SteamConsts.RegistryInstallKey);
        Result installPathResult = Result.FailIf(installPath is null, new RegistryValueNotFound(steamKey, SteamConsts.RegistryInstallKey));

        string? language = steamKey.GetValueToString(SteamConsts.RegistryLanguageKey);
        Result languageResult = Result.FailIf(language is null, new RegistryValueNotFound(steamKey, SteamConsts.RegistryInstallKey));

        Result<SteamInstallationInfo> valueResults = Result.Merge(installPathResult, languageResult)
            .ToResult<SteamInstallationInfo>();

        if (valueResults.IsFailed) return valueResults;

        return new SteamInstallationInfo(
            InstallPath: installPath!,
            Language: language!,
            AppsIds: apps.GetSubKeyNames());
    }

    private static string? GetValueToString(this RegistryKey registryKey, string valueKey)
    {
        return (string?)registryKey.GetValue(valueKey);
    }
}
