namespace Emyfreya.Steam.Models.Errors;

public sealed class RegistrySubKeyNotFound(RegistryKey baseKey, string name)
    : Error($"The registry sub key couldn't be found from '{baseKey.Name}' with '{name}'.");
