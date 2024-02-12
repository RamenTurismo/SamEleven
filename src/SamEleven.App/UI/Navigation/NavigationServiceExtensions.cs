namespace SamEleven.App.UI;

internal static class NavigationServiceExtensions
{
    public static INavigationServiceBuilder AddNavigationService(this IServiceCollection services, Frame frame)
    {
        services.AddSingleton<INavigationService>(p => new NavigationService(
            p.GetRequiredService<IServiceScopeFactory>(), 
            frame, 
            p.GetRequiredService<WeakReferenceMessenger>(),
            p.GetRequiredService<ILogger<NavigationService>>()));

        return new NavigationServiceBuilder(services);
    }
}
