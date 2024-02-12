namespace SamEleven.App.UI.Navigation;

internal sealed class NavigationServiceBuilder : INavigationServiceBuilder
{
    private readonly IServiceCollection _services;

    public NavigationServiceBuilder(IServiceCollection services)
    {
        _services = services;
    }

    public INavigationServiceBuilder AddView<TView, TViewModel>(ServiceLifetime lifetime = ServiceLifetime.Singleton)
        where TView : FrameworkElement
        where TViewModel : class
    {
        _services.Add(new ServiceDescriptor(typeof(TViewModel), typeof(TViewModel), lifetime));
        _services.Add(new ServiceDescriptor(typeof(TView), typeof(TView), lifetime));
        _services.Add(new ServiceDescriptor(typeof(FrameworkElement), typeof(TViewModel), (p, k) => p.GetRequiredService<TView>(), lifetime));

        return this;
    }
}
