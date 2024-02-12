namespace Emyfreya.Steam.Desktop;

[UnmanagedFunctionPointer(CallingConvention.ThisCall, CharSet = CharSet.Ansi)]
internal delegate int GetAppData(nint self, uint appId, StringBuilder key, StringBuilder value, int valueLength);

[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
[return: MarshalAs(UnmanagedType.I1)]
internal delegate bool IsSubscribedApp(nint self, uint appId);
