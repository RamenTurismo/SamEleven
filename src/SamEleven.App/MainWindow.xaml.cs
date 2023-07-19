using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SamEleven.App.Achievement;
using SamEleven.App.Picker;

namespace SamEleven.App;

public sealed partial class MainWindow : Window
{
    private readonly MainWindowViewModel _viewModel;
    private readonly Frame _frame;

    public MainWindow(MainWindowViewModel viewModel, Frame frame, GamePickerViewModel gamePickerViewModel)
    {
        InitializeComponent();

        if (Content is FrameworkElement frameworkElement)
        {
            frameworkElement.DataContext = viewModel;
        }
        NavigationView.Content = frame;
        frame.Content = new GamePicker(gamePickerViewModel);

        viewModel.PropertyChanged += ViewModel_PropertyChanged;
        _viewModel = viewModel;
        _frame = frame;
    }

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if(e.PropertyName == nameof(MainWindowViewModel.IsAnyGameSelected) && _viewModel.IsAnyGameSelected)
        {
            _frame.Navigate(typeof(AchievementPage));
        }
    }
}
