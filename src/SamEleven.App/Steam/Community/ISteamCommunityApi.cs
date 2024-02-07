namespace SamEleven.App.Steam.Community;

public interface ISteamCommunityApi
{
    [Get("/profiles/{userId}/games?tab=all&xml=1")]
    Task<ApiResponse<UserGamesResponse>> GetAllGamesAsync(ulong userId, CancellationToken cancellationToken = default);
}
