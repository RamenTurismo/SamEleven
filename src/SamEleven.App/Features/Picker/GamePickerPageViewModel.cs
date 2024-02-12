namespace SamEleven.App.Features.Picker;

public sealed partial class GamePickerPageViewModel : ObservableObject, IRecipient<FrameNavigated>, IDisposable
{
    [ObservableProperty]
    private ObservableCollection<SteamApp> _games;

    [ObservableProperty]
    private bool _isSearchAvailable;

    private readonly Dictionary<uint, SteamApp> _gamesCache;
    private readonly WeakReferenceMessenger _messenger;
    private readonly ISteamService _steamService;
    private readonly IDispatcherQueueService _dispatcherQueue;
    private CancellationTokenSource? _searchTokenSource;

    public GamePickerPageViewModel(WeakReferenceMessenger messenger, ISteamService steamService, IDispatcherQueueService dispatcherQueue)
    {
        _gamesCache = [];
        _messenger = messenger;
        _steamService = steamService;
        _dispatcherQueue = dispatcherQueue;

        _messenger.RegisterAll(this);
        Games = new ObservableCollection<SteamApp>();
    }

    public void Dispose()
    {
        _searchTokenSource?.Cancel();
        _searchTokenSource?.Dispose();

        _messenger.UnregisterAll(this);
    }

    public void Receive(FrameNavigated message)
    {
        // So it doesn't get called more than once.
        _messenger.UnregisterAll(this);

        Task.Run(LoadGamesAsync);
    }

    public void SelectGame(SteamApp game)
    {
        _messenger.Send(new GameSelectedMessage(game));
    }

    public Task SearchAsync(string? query)
    {
        _searchTokenSource?.Cancel();
        _searchTokenSource?.Dispose();

        _searchTokenSource = new CancellationTokenSource();

        Games = new ObservableCollection<SteamApp>();
        return Task.Run(() => SearchAsync(query, _searchTokenSource.Token));
    }

    private async ValueTask LoadGamesAsync()
    {
        await foreach (SteamApp steamGame in _steamService.GetAllGamesAsync().ConfigureAwait(false))
        {
            _gamesCache.TryAdd(steamGame.Id, steamGame);

            await _dispatcherQueue.Enqueue(() => Games.Add(steamGame)).ConfigureAwait(false);
        }

        await _dispatcherQueue.Enqueue(() => IsSearchAvailable = true).ConfigureAwait(false);
    }

    private async Task SearchAsync(string? query, CancellationToken cancellationToken = default)
    {
        foreach (SteamApp item in BuildSearchQuery(query))
        {
            if (cancellationToken.IsCancellationRequested) return;

            await _dispatcherQueue.Enqueue(() => Games.Add(item), cancellationToken).ConfigureAwait(false);
        }
    }

    private IEnumerable<SteamApp> BuildSearchQuery(string? query)
    {
        IEnumerable<SteamApp> gamesQuery = _gamesCache.Values;

        if (query is not null)
        {
            gamesQuery = gamesQuery.Where(game => game.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase));
        }

        return gamesQuery;
    }
}
