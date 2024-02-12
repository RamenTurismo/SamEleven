namespace Emyfreya.Steam.Web.Community;

internal static class ServiceCollectionExtensions
{
    public static IHttpClientBuilder AddSteamCommunityApi(this IServiceCollection services)
    {
        return services.AddRefitClient<ISteamCommunityApi>(new RefitSettings
        {
            ContentSerializer = new XmlContentSerializer()
        })
        .ConfigureHttpClient((p, c) =>
        {
            p.GetRequiredService<IConfiguration>()
              .GetRequiredSection(SteamCommunityApiConfig.Key)
              .Bind(c);
        });
    }
}
