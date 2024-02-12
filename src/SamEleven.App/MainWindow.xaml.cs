namespace SamEleven.App;

public sealed partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel, Frame frame)
    {
        InitializeComponent();

        if (Content is FrameworkElement frameworkElement)
        {
            frameworkElement.DataContext = viewModel;
        }

        NavigationView.Content = frame;
    }
}
