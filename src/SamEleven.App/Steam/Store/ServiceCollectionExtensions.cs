namespace SamEleven.App.Steam.Store;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSteamStoreApi(this IServiceCollection services)
    {
        services.AddRefitClient<ISteamStoreApi>(new RefitSettings()
        {
            ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            })
        })
        .ConfigureHttpClient((p, c) =>
        {
            p.GetRequiredService<IConfiguration>()
                .GetRequiredSection(SteamStoreApiConfig.Key)
                .Bind(c);
        })
        .AddDefaultLogger();

        return services;
    }
}
