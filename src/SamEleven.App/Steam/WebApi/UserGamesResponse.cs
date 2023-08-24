using System;
using System.Xml.Serialization;

namespace SamEleven.App.Steam.WebApi;

[XmlRoot("gamesList")]
public class UserGamesResponse
{
    [XmlArray("games")]
    [XmlArrayItem("game")]
    public UserGame[] Games { get; set; } = Array.Empty<UserGame>();
}

public class UserGame
{
    [XmlElement("appID")]
    public uint AppId { get; set; }
    [XmlElement("name")]
    public string Name { get; set; } = null!;
    [XmlElement("logo")]
    public string Logo { get; set; } = null!;
}
