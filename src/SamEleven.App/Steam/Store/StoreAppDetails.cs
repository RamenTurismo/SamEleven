namespace SamEleven.App.Steam.Store;

public sealed record StoreAppDetails(bool Success, StoreAppDetailsData Data);

public sealed record StoreAppDetailsData(
    string Type,
    string Name,
    string HeaderImage,
    string CapsuleImage);
