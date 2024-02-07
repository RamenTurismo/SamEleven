namespace SamEleven.App.Steam.DesktopApi;

public sealed class SteamInstallationInfoException : Exception
{
    public SteamInstallationInfoException(string? message) : base(message)
    {
    }
}
