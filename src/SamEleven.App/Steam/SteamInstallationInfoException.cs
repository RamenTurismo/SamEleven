namespace SamEleven.Steamworks;

public sealed class SteamInstallationInfoException : Exception
{
    public SteamInstallationInfoException(string? message) : base(message)
    {
    }
}
