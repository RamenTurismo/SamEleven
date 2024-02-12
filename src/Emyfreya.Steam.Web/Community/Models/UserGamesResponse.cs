namespace Emyfreya.Steam.Web.Community.Models;

[XmlRoot("gamesList")]
public sealed record UserGamesResponse
{
    [XmlArray("games")]
    [XmlArrayItem("game")]
    public IEnumerable<UserGame> Games { get; init; } = Enumerable.Empty<UserGame>();
}

public sealed record UserGame
{
    [XmlElement("appID")]
    public uint AppId { get; set; }
    [XmlElement("name")]
    public string Name { get; set; } = null!;
    [XmlElement("logo")]
    public string Logo { get; set; } = null!;
}
