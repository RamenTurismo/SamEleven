namespace SamEleven.App.Abstractions;

public interface ISteamService
{
    IAsyncEnumerable<SteamGameInfo> GetAllGamesAsync(CancellationToken cancellationToken = default);
}
