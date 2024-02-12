namespace Emyfreya.Steam.Web.Api;

public static class ServiceCollectionExtensions
{
    public static IHttpClientBuilder AddSteamApi(this IServiceCollection services)
    {
        JsonSerializerOptions options = new()
        {
            TypeInfoResolver = SteamApiJsonContext.Default
        };

        return services.AddRefitClient<ISteamApi>(new RefitSettings()
        {
            ContentSerializer = new SystemTextJsonContentSerializer(new JsonSerializerOptions(options)
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            })
        })
        .ConfigureHttpClient((p, c) =>
        {
            p.GetRequiredService<IConfiguration>()
                .GetRequiredSection(SteamApiConfig.Key)
                .Bind(c);
        });
    }
}
