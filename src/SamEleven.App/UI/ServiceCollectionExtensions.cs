namespace SamEleven.App.UI;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDispatcherQueueService(this IServiceCollection services)
    {
        services.TryAddSingleton<IDispatcherQueueService, DispatcherQueueService>();

        return services;
    }
}
