namespace SamEleven.App.Features.Picker;

public sealed partial class GamePickerViewModel : ObservableObject, IDisposable
{
    [ObservableProperty]
    private ObservableCollection<SteamGameInfo> _games;

    [ObservableProperty]
    private bool _isSearchAvailable;

    private readonly Dictionary<uint, SteamGameInfo> _gamesCache;
    private readonly WeakReferenceMessenger _messenger;
    private readonly ISteamService _steamService;
    private readonly IDispatcherQueueService _dispatcherQueue;
    private CancellationTokenSource? _searchTokenSource;

    public GamePickerViewModel(WeakReferenceMessenger messenger, ISteamService steamService, IDispatcherQueueService dispatcherQueue)
    {
        Games = new ObservableCollection<SteamGameInfo>();
        _gamesCache = [];
        _messenger = messenger;
        _steamService = steamService;
        _dispatcherQueue = dispatcherQueue;
    }

    internal async Task InitializeAsync()
    {
        await Task.Run(LoadGamesAsync).ConfigureAwait(true);
    }

    private async ValueTask LoadGamesAsync()
    {
        await foreach (SteamGameInfo steamGame in _steamService.GetAllGamesAsync().ConfigureAwait(false))
        {
            _gamesCache.TryAdd(steamGame.Id, steamGame);

            await _dispatcherQueue.Enqueue(() => Games.Add(steamGame)).ConfigureAwait(false);
        }

        await _dispatcherQueue.Enqueue(() => IsSearchAvailable = true).ConfigureAwait(false);
    }

    public void SelectGame(SteamGameInfo game)
    {
        _messenger.Send(new GameSelectedMessage(game));
    }

    public Task SearchAsync(string? query)
    {
        _searchTokenSource?.Cancel();
        _searchTokenSource?.Dispose();

        _searchTokenSource = new CancellationTokenSource();

        Games = new ObservableCollection<SteamGameInfo>();
        return Task.Run(() => SearchAsync(query, _searchTokenSource.Token));
    }

    private async Task SearchAsync(string? query, CancellationToken cancellationToken = default)
    {
        foreach (SteamGameInfo item in BuildSearchQuery(query))
        {
            if (cancellationToken.IsCancellationRequested) return;

            await _dispatcherQueue.Enqueue(() => Games.Add(item), cancellationToken).ConfigureAwait(false);
        }
    }

    private IEnumerable<SteamGameInfo> BuildSearchQuery(string? query)
    {
        IEnumerable<SteamGameInfo> gamesQuery = _gamesCache.Values;

        if (query is not null)
        {
            gamesQuery = gamesQuery.Where(game => game.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase));
        }

        return gamesQuery;
    }

    public void Dispose()
    {
        _searchTokenSource?.Cancel();
        _searchTokenSource?.Dispose();
    }
}
