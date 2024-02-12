namespace Emyfreya.Steam.Desktop.Models.Errors;

public sealed class AppDataNotFound : Error
{
    public AppDataNotFound(uint appId, string key, int resultLength)
        : base($"AppData not found for '{appId}' with key '{key}'.")
    {
        WithMetadata("resultLength", resultLength);
    }
}
