namespace RamenTurismo.Steam.Models.Errors;

public sealed class SteamCreateInterfaceCodeError :Error
{
    public SteamCreateInterfaceCodeError(nint handle, string version, uint code) 
        : base($"Could not create interface {version}.")
    {
        WithMetadata("Code", code);
        WithMetadata("Handle", handle);
    }
}
