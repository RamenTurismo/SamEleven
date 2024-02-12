namespace Emyfreya.Steam.Models.Errors;

public sealed class ConnectToGlobalUserError(int pipe) 
    : Error($"Could not connect to global user with pipe {pipe}.");
