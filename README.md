# SAM Eleven

Otherwise called "Steam Achievement Manager Eleven", is a project derived from [gibbed's SteamAchievementManager](https://github.com/gibbed/SteamAchievementManager).

This project is an updated version, with the latest .NET technologies (WinUI, .NET 7), made specifically to follow Windows 11's UI and ergonomy guidelines.

## What changes

### User wise

This app is now composed of only one executable, and is thought out to be an SPA (Single Page Application).

### Technical

The Steam API has been remade from scratch, to be adapted to .NET 7 new NativeLibrary communication, and to include DI and testability in mind.

## Steam API Native Library : path to remake

- [Interacting with native libraries in .NET Core 3.0](https://developers.redhat.com/blog/2019/09/06/interacting-with-native-libraries-in-net-core-3-0)
- [Platform Invoke (P/Invoke)](https://learn.microsoft.com/en-us/dotnet/standard/native-interop/pinvoke)
- [Facepunch.Steamworks](https://github.com/Facepunch/Facepunch.Steamworks/blob/master/Facepunch.Steamworks/SteamClient.cs)
- [Steamworks API Overview](https://partner.steamgames.com/doc/sdk/api)
- [Steam4NET](https://github.com/SteamRE/Steam4NET)
