namespace Emyfreya.Steam.Web.Store.Models;

public sealed record StoreAppDetails(bool Success, StoreAppDetailsData Data);

public sealed record StoreAppDetailsData(
    string Type,
    string Name,
    string HeaderImage,
    string CapsuleImage);
