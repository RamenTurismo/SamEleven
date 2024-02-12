namespace SamEleven.App.Caching;

internal static class FileCacheServiceExtensions
{
    public static IServiceCollection AddFileCacheService(this IServiceCollection services, Action<FileCacheServiceOptions> options)
    {
        services.AddOptions<FileCacheServiceOptions>()
            .Configure(options)
            .Validate(o => o.RootPath is { Length: > 3 });

        return services.AddSingleton<IFileCacheService, FileCacheService>();
    }
}
