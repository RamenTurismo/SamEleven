namespace SamEleven.App;

public sealed partial class MainWindow : Window, IRecipient<FrameNavigated>, IDisposable
{
    private readonly MainWindowViewModel _viewModel;
    private readonly WeakReferenceMessenger _messenger;

    public MainWindow(MainWindowViewModel viewModel, Frame frame, WeakReferenceMessenger messenger)
    {
        InitializeComponent();

        if (Content is FrameworkElement frameworkElement)
        {
            frameworkElement.DataContext = viewModel;
        }

        NavigationView.Content = frame;
        _viewModel = viewModel;
        _messenger = messenger;
        messenger.Register(this);
    }

    public void Dispose()
    {
        _messenger.UnregisterAll(this);
    }

    public void Receive(FrameNavigated message)
    {
        NavigationView.SelectedItem = NavigationView.MenuItems
            .OfType<FrameworkElement>()
            .Where(e => _viewModel.GetViewModelType(e.Tag) == message.ViewModelType)
            .FirstOrDefault(NavigationView.MenuItems[0]);
    }

    private void NavigationViewItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        _viewModel.RequestNavigationAsync(args.InvokedItemContainer.Tag, args.IsSettingsInvoked);
    }
}
