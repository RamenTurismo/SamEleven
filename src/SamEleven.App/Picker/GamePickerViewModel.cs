namespace SamEleven.App.Picker;

public sealed partial class GamePickerViewModel : ObservableObject
{
    [ObservableProperty]
    private IReadOnlyCollection<SteamGameInfo> _games;

    private readonly Dictionary<uint, SteamGameInfo> _gamesCache;
    private readonly WeakReferenceMessenger _messenger;
    private readonly ISteamService _steamService;

    public GamePickerViewModel(WeakReferenceMessenger messenger, ISteamService steamService)
    {
        Games = Array.Empty<SteamGameInfo>();
        _gamesCache = new Dictionary<uint, SteamGameInfo>();
        _messenger = messenger;
        _steamService = steamService;
    }

    internal async Task InitializeAsync()
    {
        await LoadGamesAsync().ConfigureAwait(true);
        Search(query: null);
    }

    private async ValueTask LoadGamesAsync()
    {
        IReadOnlyList<SteamGameInfo> steamGames = await _steamService.GetAllGamesAsync().ConfigureAwait(true);

        foreach (SteamGameInfo steamGame in steamGames)
        {
            _gamesCache.TryAdd(steamGame.Id, steamGame);
        }
    }

    public void SelectGame(SteamGameInfo game)
    {
        _messenger.Send(new GameSelectedMessage(game));
    }

    public void Search(string? query)
    {
        IEnumerable<SteamGameInfo> gamesQuery = _gamesCache.Values;

        if (query is not null)
        {
            gamesQuery = gamesQuery.Where(game => game.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase));
        }

        Games = gamesQuery.ToArray();
    }
}
