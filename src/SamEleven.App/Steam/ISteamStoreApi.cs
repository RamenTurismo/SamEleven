namespace SamEleven.App.Steam;

public interface ISteamStoreApi
{
    [Get("/appdetails")]
    Task GetAppInfo([Query] [AliasAs("appids")] uint appId);
}
