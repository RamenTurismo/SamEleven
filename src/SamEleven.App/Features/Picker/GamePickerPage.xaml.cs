// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

namespace SamEleven.App.Features.Picker;

public sealed partial class GamePickerPage : UserControl
{
    private readonly GamePickerPageViewModel _viewModel;

    public GamePickerPage(GamePickerPageViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        _viewModel = viewModel;
    }

    private void AutoSuggestBoxTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            _viewModel.SearchAsync(sender.Text);
        }
    }

    private void AutoSuggestBoxQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (args.ChosenSuggestion is SteamApp searchItem)
        {
            _viewModel.SelectGame(searchItem);
        }
        else
        {
            _viewModel.SearchAsync(args.QueryText);
        }
    }

    private void AutoSuggestBoxSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
    {
        SelectItem(args.SelectedItem);
    }

    private void ListViewItemClick(object sender, ItemClickEventArgs e)
    {
        SelectItem(e.ClickedItem);
    }

    private void SelectItem(object e)
    {
        if (e is SteamApp searchItem)
        {
            _viewModel.SelectGame(searchItem);
        }
    }
}
