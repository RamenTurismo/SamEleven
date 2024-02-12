namespace Emyfreya.Steam.Desktop.Client;

[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
internal delegate uint GetNumInstalledApps(nint self);

[UnmanagedFunctionPointer(CallingConvention.ThisCall)]
internal delegate uint GetInstalledApps(nint self, uint[] appIds, uint maxAppIds);
