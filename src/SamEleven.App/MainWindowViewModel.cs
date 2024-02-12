namespace SamEleven.App;

public sealed partial class MainWindowViewModel : ObservableObject, IDisposable, IRecipient<GameSelectedMessage>
{
    public bool IsAnyGameSelected => SteamGameInfo is not null;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsAnyGameSelected))]
    private SteamApp? _steamGameInfo;

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

    internal Task RequestNavigationAsync(object tag, bool isSettingsInvoked)
    {
        Type viewModelType = MapViewModelType(tag, isSettingsInvoked);

        return _navigationService.NavigateAsync(viewModelType);
    }

    internal Type GetViewModelType(object tag) => MapViewModelType(tag, isSettingsInvoked: false);

    private static Type MapViewModelType(object tag, bool isSettingsInvoked)
    {
        if (isSettingsInvoked) return typeof(GamePickerPageViewModel);

        return tag switch
        {
            "achievements" => typeof(AchievementPageViewModel),
            _ => typeof(GamePickerPageViewModel),
        };
    }
}
