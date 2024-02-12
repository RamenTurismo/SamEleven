namespace SamEleven.App;

public sealed partial class MainWindowViewModel : ObservableObject, IDisposable, IRecipient<GameSelectedMessage>
{
    public bool IsAnyGameSelected => SteamGameInfo is not null;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAnyGameSelected))]
    private SteamGameInfo? _steamGameInfo;

    private readonly WeakReferenceMessenger _messenger;
    private readonly INavigationService _navigationService;

    public MainWindowViewModel(WeakReferenceMessenger messenger, INavigationService navigationService)
    {
        _messenger = messenger;
        _navigationService = navigationService;
    }

    public void Receive(GameSelectedMessage message)
    {
        SteamGameInfo = message.Game;

        _navigationService.NavigateAsync<AchievementPageViewModel>();
    }

    public void Dispose()
    {
        _messenger.UnregisterAll(this);
    }

    internal void Initialize()
    {
        _messenger.Register(this);
    }
}
