﻿namespace SamEleven.App.UI.Navigation;

internal sealed partial class NavigationService : INavigationService
{
    private static partial class Log
    {
        [LoggerMessage(LogLevel.Debug, "Changing content with Key {ViewModel}")]
        public static partial void ContentChanging(ILogger logger, Type viewModel);
        [LoggerMessage(LogLevel.Information, "Changed content in {Elapsed}ms to Key {ViewModel} and view {View}")]
        public static partial void ContentChanged(ILogger logger, long elapsed, Type viewModel, Type view);
    }

    private AsyncServiceScope? CurrentScope { get; set; }

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly Frame _frame;
    private readonly WeakReferenceMessenger _weakReferenceMessenger;
    private readonly ILogger _logger;

    public NavigationService(IServiceScopeFactory scopeFactory, Frame frame, WeakReferenceMessenger weakReferenceMessenger, ILogger<NavigationService> logger)
    {
        _scopeFactory = scopeFactory;
        _frame = frame;
        _weakReferenceMessenger = weakReferenceMessenger;
        _logger = logger;
    }

    public ValueTask DisposeAsync()
    {
        return CurrentScope?.DisposeAsync() ?? ValueTask.CompletedTask;
    }

    public Task NavigateAsync<TViewModel>() where TViewModel : ObservableObject
        => NavigateAsync(typeof(TViewModel));

    public async Task NavigateAsync(Type viewModelType)
    {
        if (CurrentScope is { } oldScope)
        {
            await oldScope.DisposeAsync().ConfigureAwait(true);
        }

        Log.ContentChanging(_logger, viewModelType);
        Stopwatch stopwatch = Stopwatch.StartNew();

        CurrentScope = _scopeFactory.CreateAsyncScope();
        IServiceProvider provider = CurrentScope.Value.ServiceProvider;

        FrameworkElement page = provider.GetRequiredKeyedService<FrameworkElement>(viewModelType);

        _frame.Content = page;

        Log.ContentChanged(_logger, stopwatch.ElapsedMilliseconds, viewModelType, page.GetType());

        _weakReferenceMessenger.Send(new FrameNavigated());
    }
}
