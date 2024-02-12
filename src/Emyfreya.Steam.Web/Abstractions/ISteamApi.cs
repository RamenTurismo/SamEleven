namespace Emyfreya.Steam.Web.Abstractions;

public interface ISteamApi
{
    [Get("/ISteamApps/GetAppList/{version}")]
    Task<ApiResponse<GetAppListResult>> GetAppListAsync(string version = "v2", CancellationToken cancellationToken = default);
}
