namespace RamenTurismo.Steam;

public sealed record SteamInstallationInfo(
    string InstallPath,
    string Language,
    IEnumerable<string> AppsIds);
