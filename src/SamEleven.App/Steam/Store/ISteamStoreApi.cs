namespace SamEleven.App.Steam;

/// <summary>
/// Not meant for public comsuption.
/// https://wiki.teamfortress.com/wiki/User:RJackson/StorefrontAPI
/// </summary>
public interface ISteamStoreApi
{
    [Get("/appdetails")]
    Task<ApiResponse<Dictionary<string, StoreAppDetails>>> GetAppDetailsAsync([Query(",")] [AliasAs("appids")] string[] appIds, CancellationToken cancellationToken = default);
}
