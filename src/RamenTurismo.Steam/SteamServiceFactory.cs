namespace RamenTurismo.Steam;

public static class SteamServiceFactory
{
    public static ISteamService Build()
    {
        return new SteamService();
    }
}
