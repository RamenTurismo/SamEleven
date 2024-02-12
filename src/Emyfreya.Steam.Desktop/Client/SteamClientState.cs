namespace Emyfreya.Steam.Desktop.Client;

public sealed record SteamClientState
{
    public int User { get; init; }
    public int Pipe { get; init; }

    public bool IsUserDefined => User != 0;
    public bool IsPipeDefined => Pipe != 0;
    public bool IsInitialized => IsPipeDefined && IsUserDefined;
}
