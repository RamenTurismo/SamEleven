namespace RamenTurismo.Steam;

public delegate nint CreateInterface(string version, ref uint resultCode);

[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
public delegate int CreateSteamPipe(nint self);

[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
public delegate int ConnectToGlobalUser(nint self, int pipe);

[UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
public delegate nint GetISteamApps(nint self, int user, int pipe, string version);

[UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
public delegate int GetAppData(nint self, uint appId, StringBuilder key, StringBuilder value, int valueLength);
