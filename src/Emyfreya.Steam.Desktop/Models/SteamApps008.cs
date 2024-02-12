﻿namespace Emyfreya.Steam.Desktop.Models;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
internal struct SteamApps008
{
    public const string Name = "STEAMAPPS_INTERFACE_VERSION008";

    public nint IsSubscribed;
    public nint IsLowViolence;
    public nint IsCybercafe;
    public nint IsVACBanned;
    public nint GetCurrentGameLanguage;
    public nint GetAvailableGameLanguages;
    public nint IsSubscribedApp;
    public nint IsDlcInstalled;
    public nint GetEarliestPurchaseUnixTime;
    public nint IsSubscribedFromFreeWeekend;
    public nint GetDLCCount;
    public nint GetDLCDataByIndex;
    public nint InstallDLC;
    public nint UninstallDLC;
    public nint RequestAppProofOfPurchaseKey;
    public nint GetCurrentBetaName;
    public nint MarkContentCorrupt;
    public nint GetInstalledDepots;
    public nint GetAppInstallDir;
    public nint IsAppInstalled;
    public nint GetAppOwner;
    public nint GetLaunchQueryParam;
    public nint GetDlcDownloadProgress;
    public nint GetAppBuildId;
    public nint RequestAllProofOfPurchaseKeys;
    public nint GetFileDetails;
    public nint GetLaunchCommandLine;
    public nint IsSubscribedFromFamilySharing;
}
