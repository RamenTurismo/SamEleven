namespace Emyfreya.Steam.Desktop.Client;

/// <summary>
/// The client that works around <c>steamclient.dll</c>.
/// </summary>
internal sealed class SteamClient : ISteamClient
{
    public Result<ISteamApps> SteamApps => _steamApps.Value;
    public Result<ISteamAppList> SteamAppList => _steamAppList.Value;

    private SteamClientState _state;
    private readonly VirtualClassWrapper<SteamClient018> _wrapper;
    private readonly Lazy<Result<ISteamApps>> _steamApps;
    private readonly Lazy<Result<ISteamAppList>> _steamAppList;

    private SteamClient(VirtualClassWrapper<SteamClient018> wrapper)
    {
        _state = new SteamClientState();
        _wrapper = wrapper;
        _steamApps = new(CreateSteamApps, isThreadSafe: false);
        _steamAppList = new(CreateSteamAppList, isThreadSafe: false);
    }

    public void Dispose()
    {
        if (_state.IsInitialized)
        {
            ReleaseUser(_state.Pipe, _state.User);
            ReleaseSteamPipe(_state.Pipe);
        }
    }

    public static Result<ISteamClient> Build(nint handle)
    {
        Result<VirtualClassWrapper<SteamClient018>> steamClient018 = CreateInterface<SteamClient018>(handle, SteamClient018.Name);
        if (steamClient018.IsFailed) return steamClient018.ToResult<ISteamClient>();

        return new SteamClient(steamClient018.Value);
    }

    public Result<int> CreateSteamPipe()
    {
        int pipe = _wrapper.GetDelegate<CreateSteamPipe>(v => v.CreateSteamPipe)(_wrapper.InterfaceHandle);

        if (pipe == 0) return Result.Fail(new CreatePipeError());

        return pipe;
    }

    public Result<int> ConnectToGlobalUser(int pipe)
    {
        int user = _wrapper.GetDelegate<ConnectToGlobalUser>(v => v.ConnectToGlobalUser)(_wrapper.InterfaceHandle, pipe);

        if (user == 0) return Result.Fail(new ConnectToGlobalUserError(pipe));

        return user;
    }

    public void ReleaseSteamPipe(int pipe)
    {
        _wrapper.GetDelegate<ReleaseSteamPipe>(v => v.ReleaseSteamPipe)(_wrapper.InterfaceHandle, pipe);
    }

    public void ReleaseUser(int pipe, int user)
    {
        _wrapper.GetDelegate<ReleaseUser>(v => v.ReleaseUser)(_wrapper.InterfaceHandle, pipe, user);
    }

    private Result<SteamClientState> GetOrCreateState()
    {
        if (!_state.IsPipeDefined)
        {
            Result<int> pipe = CreateSteamPipe();
            if (pipe.IsFailed) return pipe.ToResult();

            _state = _state with { Pipe = pipe.Value };
        }

        if (!_state.IsUserDefined)
        {
            Result<int> user = ConnectToGlobalUser(_state.Pipe);
            if (user.IsFailed) return user.ToResult();

            _state = _state with { User = user.Value };
        }

        return _state;
    }

    private Result<ISteamApps> CreateSteamApps()
    {
        return GetOrCreateState()
            .Bind(state => CreateSteamApps(state.User, state.Pipe));
    }
    private Result<ISteamAppList> CreateSteamAppList()
    {
        return GetOrCreateState()
            .Bind(state => CreateSteamAppList(state.User, state.Pipe));
    }

    private Result<ISteamApps> CreateSteamApps(int user, int pipe)
    {
        nint steamApps001Handle = _wrapper.GetDelegate<GetISteamApps>(v => v.GetISteamApps)(_wrapper.InterfaceHandle, user, pipe, SteamApps001.Name);
        Result<VirtualClassWrapper<SteamApps001>> steamApps001 = CreateVirtualClassWrapper<SteamApps001>(steamApps001Handle);

        nint steamApps008Handle = _wrapper.GetDelegate<GetISteamApps>(v => v.GetISteamApps)(_wrapper.InterfaceHandle, user, pipe, SteamApps008.Name);
        Result<VirtualClassWrapper<SteamApps008>> steamApps008 = CreateVirtualClassWrapper<SteamApps008>(steamApps008Handle);

        return Result.Merge(steamApps001, steamApps008)
            .Bind<ISteamApps>(() => new SteamApps(steamApps001.Value, steamApps008.Value));
    }

    private Result<ISteamAppList> CreateSteamAppList(int user, int pipe)
    {
        nint handle = _wrapper.GetDelegate<GetISteamAppList>(v => v.GetISteamAppList)(_wrapper.InterfaceHandle, user, pipe, SteamAppList001.Name);

        return CreateVirtualClassWrapper<SteamAppList001>(handle)
            .Bind<ISteamAppList>(w => new SteamAppList(w));
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

