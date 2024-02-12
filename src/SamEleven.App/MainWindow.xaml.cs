namespace SamEleven.App;

public sealed partial class MainWindow : Window
{
    private readonly MainWindowViewModel _viewModel;

    public MainWindow(MainWindowViewModel viewModel, Frame frame)
    {
        InitializeComponent();

        if (Content is FrameworkElement frameworkElement)
        {
            frameworkElement.DataContext = viewModel;
        }

        NavigationView.Content = frame;
        _viewModel = viewModel;
    }

    private void NavigationViewItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
    {
        _viewModel.RequestNavigationAsync(args.InvokedItemContainer.Tag, args.IsSettingsInvoked);
    }
}
