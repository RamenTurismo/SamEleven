namespace SamEleven.App.Abstractions;

public interface ISteamService
{
    ValueTask<SteamGameInfo[]> GetAllGamesAsync(CancellationToken cancellationToken = default);
}
