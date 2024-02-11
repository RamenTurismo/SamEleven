namespace Emyfreya.Steam.Desktop;

internal delegate nint CreateInterface(string version, ref uint resultCode);

[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
internal delegate int CreateSteamPipe(nint self);

[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
internal delegate int ConnectToGlobalUser(nint self, int pipe);

[UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
internal delegate nint GetISteamApps(nint self, int user, int pipe, string version);

[UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
internal delegate int GetAppData(nint self, uint appId, StringBuilder key, StringBuilder value, int valueLength);
