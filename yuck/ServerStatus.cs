using Newtonsoft.Json;

namespace yuck;

public class ServerStatus
{
    [JsonProperty("version")]
    public VersionInfo? Version { get; set; }

    [JsonProperty("players")]
    public PlayerInfo? Players { get; set; }

    [JsonProperty("description")]
    public Description? Description { get; set; }

    [JsonProperty("favicon")]
    public string? Favicon { get; set; }

    [JsonProperty("enforcesSecureChat")]
    public bool EnforcesSecureChat { get; set; }
}

public class VersionInfo
{
    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("protocol")]
    public int Protocol { get; set; }
}

public class PlayerInfo
{
    [JsonProperty("max")]
    public int Max { get; set; }

    [JsonProperty("online")]
    public int Online { get; set; }

    [JsonProperty("sample")]
    public List<PlayerSample>? Sample { get; set; }
}

public class PlayerSample
{
    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("id")]
    public string? Id { get; set; }
}

[JsonConverter(typeof(DescriptionConverter))]
public class Description
{
    [JsonProperty("text")]
    public string? Text { get; set; }
}
