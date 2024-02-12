namespace SamEleven.App.Picker;

public sealed partial class GamePickerViewModel : ObservableObject, IDisposable
{
    [ObservableProperty]
    private ObservableCollection<SteamGameInfo> _games;

    private readonly Dictionary<uint, SteamGameInfo> _gamesCache;
    private readonly WeakReferenceMessenger _messenger;
    private readonly ISteamService _steamService;
    private readonly TaskScheduler _taskScheduler;
    private CancellationTokenSource? _searchTokenSource;

    public GamePickerViewModel(WeakReferenceMessenger messenger, ISteamService steamService)
    {
        Games = new ObservableCollection<SteamGameInfo>();
        _gamesCache = [];
        _messenger = messenger;
        _steamService = steamService;
        _taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
    }

    internal async Task InitializeAsync()
    {
        await Task.Run(LoadGamesAsync).ConfigureAwait(true);
    }

    private async ValueTask LoadGamesAsync()
    {
        await foreach (SteamGameInfo steamGame in _steamService.GetAllGamesAsync().ConfigureAwait(true))
        {
            _gamesCache.TryAdd(steamGame.Id, steamGame);

            _ = Task.Factory.StartNew(() => Games.Add(steamGame), default, TaskCreationOptions.None, _taskScheduler);
        }
    }

    public void SelectGame(SteamGameInfo game)
    {
        _messenger.Send(new GameSelectedMessage(game));
    }

    public void Search(string? query)
    {
        _searchTokenSource?.Cancel();
        _searchTokenSource?.Dispose();

        _searchTokenSource = new CancellationTokenSource();

        Search(query, _searchTokenSource.Token);
    }

    private void Search(string? query, CancellationToken cancellationToken)
    {
        if (cancellationToken.IsCancellationRequested) return;

        IEnumerable<SteamGameInfo> gamesQuery = _gamesCache.Values;

        if (query is not null)
        {
            gamesQuery = gamesQuery.Where(game => game.Name.Contains(query, StringComparison.InvariantCultureIgnoreCase));
        }

        if (cancellationToken.IsCancellationRequested) return;

        Games = new ObservableCollection<SteamGameInfo>();

        foreach (SteamGameInfo item in gamesQuery)
        {
            if (cancellationToken.IsCancellationRequested) return;

            Games.Add(item);
        }
    }

    public void Dispose()
    {
        _searchTokenSource?.Cancel();
        _searchTokenSource?.Dispose();
    }
}
