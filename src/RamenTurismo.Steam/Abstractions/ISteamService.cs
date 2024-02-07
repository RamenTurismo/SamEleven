namespace RamenTurismo.Steam.Abstractions;

public interface ISteamService
{
    public string? GetAppName(uint appId);
}
