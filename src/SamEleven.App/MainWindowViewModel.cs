using System;
using CommunityToolkit.Mvvm.Messaging;
using SamEleven.App.Picker;

namespace SamEleven.App;

public sealed partial class MainWindowViewModel : ObservableObject, IDisposable, IRecipient<GameSelectedMessage>
{
    [ObservableProperty]
    private bool _isAnyGameSelected;

    private readonly WeakReferenceMessenger _messenger;

    public MainWindowViewModel(WeakReferenceMessenger messenger)
    {
        _messenger = messenger;
    }

    public void Receive(GameSelectedMessage message)
    {
        IsAnyGameSelected = message.Game is not null;
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
