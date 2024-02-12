namespace Emyfreya.Steam.Web.Abstractions;

/// <summary>
/// Not meant for public comsuption.
/// See: https://wiki.teamfortress.com/wiki/User:RJackson/StorefrontAPI
/// </summary>
public interface ISteamStoreApi
{
    [Get("/appdetails")]
    Task<ApiResponse<Dictionary<string, StoreAppDetails>>> GetAppDetailsAsync([Query(",")][AliasAs("appids")] string[] appIds, CancellationToken cancellationToken = default);
}
