namespace RamenTurismo.Steam;

/// <summary>
/// The client that works around <c>steamclient.dll</c>.
/// </summary>
internal sealed class SteamClient
{
    public Result<SteamApps> SteamApps => _steamAppsLazy.Value;

    private readonly VirtualClassWrapper<SteamClient018> _wrapper;
    private readonly Lazy<Result<SteamApps>> _steamAppsLazy;

    private SteamClient(VirtualClassWrapper<SteamClient018> wrapper)
    {
        _wrapper = wrapper;
        _steamAppsLazy = new Lazy<Result<SteamApps>>(CreateSteamApps, isThreadSafe: false);
    }

    public static Result<SteamClient> Build(nint handle)
    {
        Result<VirtualClassWrapper<SteamClient018>> steamClient018 = CreateInterface<SteamClient018>(handle, SteamClient018.Name);
        if (steamClient018.IsFailed) return steamClient018.ToResult<SteamClient>();

        return new SteamClient(steamClient018.Value);
    }

    public int CreateSteamPipe()
    {
        return _wrapper.GetDelegate<CreateSteamPipe>(v => v.CreateSteamPipe)(_wrapper.InterfaceHandle);
    }

    public int ConnectToGlobalUser(int pipe)
    {
        return _wrapper.GetDelegate<ConnectToGlobalUser>(v => v.ConnectToGlobalUser)(_wrapper.InterfaceHandle, pipe);
    }

    private Result<SteamApps> CreateSteamApps()
    {
        int pipe = CreateSteamPipe();
        int user = ConnectToGlobalUser(pipe);

        return CreateSteamApps(user, pipe);
    }

    private Result<SteamApps> CreateSteamApps(int user, int pipe)
    {
        Result validationResult = Result.Merge([
            Result.FailIf(user == 0, new CreateSteamAppsError().WithMetadata(nameof(user), user)),
            Result.FailIf(pipe == 0, new CreateSteamAppsError().WithMetadata(nameof(pipe), pipe)),
        ]);

        if (validationResult.IsFailed) return validationResult.ToResult<SteamApps>();

        nint interfaceHandle = _wrapper.GetDelegate<GetISteamApps>(v => v.GetISteamApps)(_wrapper.InterfaceHandle, user, pipe, SteamApps001.Name);
        Result<VirtualClassWrapper<SteamApps001>> virtualClassWrapper = CreateVirtualClassWrapper<SteamApps001>(interfaceHandle);

        if (virtualClassWrapper.IsFailed) return virtualClassWrapper.ToResult<SteamApps>();

        return new SteamApps(virtualClassWrapper.Value);
    }

    private static nint CreateInterface(nint handle, string version, out uint code)
    {
        nint createInterfacePointer = NativeLibrary.GetExport(handle, "CreateInterface");
        CreateInterface createInterfaceDelegate = Marshal.GetDelegateForFunctionPointer<CreateInterface>(createInterfacePointer);

        code = 0;
        return createInterfaceDelegate(version, ref code);
    }

    private static Result<VirtualClassWrapper<T>> CreateInterface<T>(nint handle, string version)
        where T : struct
    {
        nint interfaceHandle = CreateInterface(handle, version, out uint code);

        if (code != 0) return Result.Fail(new SteamCreateInterfaceCodeError(handle, version, code));

        return CreateVirtualClassWrapper<T>(interfaceHandle);
    }

    private static Result<VirtualClassWrapper<T>> CreateVirtualClassWrapper<T>(nint interfaceHandle)
        where T : struct
    {
        return Result.Try(() => Marshal.PtrToStructure<VirtualClass>(interfaceHandle).VirtualTable)
            .Bind(virtualClassHandle => Result.Try(() => Marshal.PtrToStructure<T>(virtualClassHandle)))
            .Bind<VirtualClassWrapper<T>>(structure => new VirtualClassWrapper<T>(structure, interfaceHandle));
    }
}

public sealed class CreateSteamAppsError() : Error();
