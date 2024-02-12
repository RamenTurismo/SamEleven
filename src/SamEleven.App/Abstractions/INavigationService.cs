namespace SamEleven.App.Abstractions;

public interface INavigationService : IAsyncDisposable
{
    Task NavigateAsync<TViewModel>() where TViewModel : ObservableObject;
    Task NavigateAsync(Type type);
}
