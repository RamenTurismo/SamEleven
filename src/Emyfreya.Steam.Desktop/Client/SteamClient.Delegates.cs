namespace Emyfreya.Steam.Desktop.Client;

internal delegate nint CreateInterface(string version, ref uint resultCode);

[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
internal delegate int CreateSteamPipe(nint self);

[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
internal delegate int ConnectToGlobalUser(nint self, int pipe);

[UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
internal delegate nint GetISteamApps(nint self, int user, int pipe, string version);

[UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
internal delegate nint GetISteamAppList(nint self, int user, int pipe, string version);

[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
internal delegate void ReleaseUser(nint self, int pipe, int user);

[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
[return: MarshalAs(UnmanagedType.I1)]
internal delegate bool ReleaseSteamPipe(nint self, int pipe);
