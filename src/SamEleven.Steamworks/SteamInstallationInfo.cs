using Microsoft.Win32;

namespace SamEleven.Steamworks;

public sealed record SteamInstallationInfo(
    string InstallPath,
    string Language,
    IReadOnlyList<string> AppsIds)
{
    public static SteamInstallationInfo FromRegistry()
    {
        // Gotta make it work for x86+x64.
        //
        // View keys :
        // https://learn.microsoft.com/en-us/troubleshoot/windows-client/deployment/view-system-registry-with-64-bit-windows#view-64-bit-and-32-bit-registry-keys
        using RegistryKey? localKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
        using RegistryKey? steamKey = localKey.OpenSubKey(SteamConsts.RegistryPath, false) ?? throw new SteamInstallationInfoException("Steam is not installed.");
        using RegistryKey? apps = steamKey.OpenSubKey(SteamConsts.RegistryAppsKey) ?? throw new SteamInstallationInfoException("No Apps key available.");

        return new SteamInstallationInfo(
            InstallPath: GetValue(steamKey, SteamConsts.RegistryInstallKey)!,
            Language: GetValue(steamKey, SteamConsts.RegistryLanguageKey)!,
            AppsIds: apps.GetSubKeyNames());

        static string GetValue(RegistryKey? registryKey, string valueKey)
        {
            ArgumentNullException.ThrowIfNull(registryKey, nameof(registryKey));

            return (string?)registryKey.GetValue(valueKey) ?? throw new SteamInstallationInfoException($"Registry: '{registryKey.Name}' does not contain '{valueKey}'.");
        }
    }
}
