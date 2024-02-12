namespace Emyfreya.Steam.Desktop;

public sealed record SteamInstallationInfo(
    string InstallPath,
    string Language,
    IEnumerable<string> AppsIds);
