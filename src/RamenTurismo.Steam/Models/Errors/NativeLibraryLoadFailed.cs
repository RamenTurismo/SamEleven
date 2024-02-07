namespace RamenTurismo.Steam.Models.Errors;

public sealed class NativeLibraryLoadFailed(string path)
    : Error($"Could not load native library from '{path}'.");
