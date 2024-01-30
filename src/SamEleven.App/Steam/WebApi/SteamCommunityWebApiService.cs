namespace SamEleven.App.Steam.WebApi;

public sealed class SteamCommunityWebApiService
{
    public const string OptionsSectionName = "SteamCommunityWebApi";

    private readonly HttpClient _httpClient;

    public SteamCommunityWebApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async ValueTask<UserGamesResponse?> GetAllSteamGamesAsync(ulong userId, CancellationToken cancellationToken = default)
    {
        XmlSerializer serializer = new(typeof(UserGamesResponse));

        using Stream stream = await _httpClient.GetStreamAsync($"profiles/{userId}/games?tab=all&xml=1", cancellationToken).ConfigureAwait(false);
        return serializer.Deserialize(stream) as UserGamesResponse;
    }
}
