namespace RamenTurismo.Steam;

internal sealed class SteamService : ISteamService
{
    public SteamService()
    {
    }

    public string? GetAppName(uint appId)
    {
        string installPath = "C:\\Program Files (x86)\\Steam";
        string dllPath = Path.Combine(installPath, "steamclient.dll");
        Result<SteamClient> steamClient = SteamClientFactory.BuildFromPath(dllPath);

        if (steamClient.IsFailed) return null;

        Result<SteamApps> steamApps = steamClient.Value.SteamApps;

        if(steamApps.IsFailed) return null;

        return steamApps.Value.GetAppName(appId);
    }
}
