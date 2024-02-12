using Emyfreya.Steam.Web.Community.Models;

namespace Emyfreya.Steam.Web.Abstractions;

public interface ISteamCommunityApi
{
    [Get("/profiles/{userId}/games?tab=all&xml=1")]
    Task<ApiResponse<UserGamesResponse>> GetAllGamesAsync(ulong userId, CancellationToken cancellationToken = default);
}
