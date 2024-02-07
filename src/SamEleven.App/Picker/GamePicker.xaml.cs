// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.

using Microsoft.UI.Xaml.Controls;

namespace SamEleven.App.Picker;

public sealed partial class GamePicker : UserControl
{
    private readonly GamePickerViewModel _viewModel;

    public GamePicker(GamePickerViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        _viewModel = viewModel;
    }

    private void AutoSuggestBoxTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
    {
        if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        {
            _viewModel.Search(sender.Text);
        }
    }

    private void AutoSuggestBoxQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        if (args.ChosenSuggestion is SteamGameInfo searchItem)
        {
            _viewModel.SelectGame(searchItem);
        }
        else
        {
            _viewModel.Search(args.QueryText);
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
        if (e is SteamGameInfo searchItem)
        {
            _viewModel.SelectGame(searchItem);
        }
    }
}
