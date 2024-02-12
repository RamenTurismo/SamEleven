namespace Emyfreya.Steam.Web.Store;

public static class ServiceCollectionExtensions
{
    public static IHttpClientBuilder AddSteamStoreApi(this IServiceCollection services)
    {
        return services.AddRefitClient<ISteamStoreApi>(new RefitSettings()
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
        });
    }
}
