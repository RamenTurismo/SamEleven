namespace RamenTurismo.Steam.Models.Errors;

public sealed class RegistryValueNotFound(RegistryKey registryKey, string name)
    : Error($"The registry '{registryKey.Name}' does not contain '{name}'.");
