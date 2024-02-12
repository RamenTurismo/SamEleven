namespace Emyfreya.Steam.Desktop.Models.Errors;

public sealed class RegistryBaseKeyNotFound(RegistryHive registryHive, RegistryView registryView)
    : Error($"The registry base key couldn't be found with '{registryHive}' and '{registryView}'.");
