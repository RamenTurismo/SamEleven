namespace SamEleven.App.Abstractions;

public interface INavigationServiceBuilder
{
    INavigationServiceBuilder AddView<TView, TViewModel>(ServiceLifetime lifetime = ServiceLifetime.Singleton)
        where TView : FrameworkElement
        where TViewModel : class;
}
