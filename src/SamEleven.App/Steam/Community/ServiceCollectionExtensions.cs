namespace SamEleven.App.Steam.Community;

internal static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSteamCommunityApi(this IServiceCollection services)
    {
        services.AddRefitClient<ISteamCommunityApi>(new RefitSettings
        {
            ContentSerializer = new XmlContentSerializer()
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
