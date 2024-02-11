namespace SamEleven.App.Steam.SteamworksSdk;

internal sealed class SteamApiClient
{
    public uint[] GetAppList()
    {
        Emyfreya.Steam.Abstractions.ISteamService steamClient = Emyfreya.Steam.SteamServiceFactory.Build();
        string name = steamClient.GetAppName(2626940).ValueOrDefault;

        return [];
    }
}

