namespace SamEleven.App.Abstractions;

public interface ISteamService
{
    IAsyncEnumerable<SteamApp> GetAllGamesAsync(CancellationToken cancellationToken = default);
}
