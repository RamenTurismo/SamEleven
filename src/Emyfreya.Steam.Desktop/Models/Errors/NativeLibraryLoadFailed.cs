namespace Emyfreya.Steam.Models.Errors;

public sealed class NativeLibraryLoadFailed : Error
{
    public NativeLibraryLoadFailed(string path, Exception ex)
        : base($"Could not load native library from '{path}'.")
    {
        CausedBy(ex);
    }
}
