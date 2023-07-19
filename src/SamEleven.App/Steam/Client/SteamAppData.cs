﻿namespace SamEleven.App.Steam.Client;

public record class SteamAppData(
    string? Name,
    string? Logo)
{
    public const string NameKey = "name";
    public const string LogoKey = "logo";
}
