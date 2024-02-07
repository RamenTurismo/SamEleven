namespace RamenTurismo.Steam;

/// <summary>
/// Global constants used in this library.
/// </summary>
internal static class SteamConsts
{
    public const string RegistryPath = @"Software\Valve\Steam";
    public const string RegistryInstallKey = "InstallPath";
    public const string RegistryLanguageKey = "Language";
    public const string RegistryAppsKey = "Apps";
    public const string SteamClientDll = "steamclient.dll";
    public const string SteamClient64Dll = "steamclient64.dll";

    public static string SteamClientDllName => Environment.Is64BitOperatingSystem ? SteamClient64Dll : SteamClientDll;
}
