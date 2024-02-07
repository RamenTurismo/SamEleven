using System.Text;

namespace SamEleven.App.Steam.SteamworksSdk;


abstract class INativeLibrary
{
    public nint InterfacePointer { get; }

    protected INativeLibrary(nint interfacePointer)
    {
        InterfacePointer = interfacePointer;
    }
}

class ISteamClient(nint interfacePointer) : INativeLibrary(interfacePointer)
{
    //public nint CreateSteamPipe()
    //{
    //    nint pipePointer = NativeLibrary.GetExport(InterfacePointer, "Steam_CreateSteamPipe");
    //    CreateSteamPipe reateSteamPipe = Marshal.GetDelegateForFunctionPointer<CreateSteamPipe>(pipePointer);

    //    return reateSteamPipe();
    //}

    //public nint ConnectToGlobalUser(nint pipe)
    //{
    //    nint userPointer = NativeLibrary.GetExport(InterfacePointer, "ConnectToGlobalUser");
    //    ConnectToGlobalUser connectToGlobalUser = Marshal.GetDelegateForFunctionPointer<ConnectToGlobalUser>(userPointer);

    //    return connectToGlobalUser(pipe);
    //}
}

internal sealed class SteamApiClient
{
    private readonly nint _pointer;

    public SteamApiClient()
    {
        // string path = Path.Combine(Directory.GetCurrentDirectory(), "steam_api.dll");
        string installPath = "C:\\Program Files (x86)\\Steam";
        string dllPath = Path.Combine(installPath, "steamclient.dll");
        _pointer = NativeLibrary.Load(dllPath);
    }

    public uint[] GetAppList()
    {
        RamenTurismo.Steam.Abstractions.ISteamService steamClient = RamenTurismo.Steam.SteamServiceFactory.Build();
        string? name = steamClient.GetAppName(420);

        return [];
    }
}

