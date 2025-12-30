using System.Text.Json.Serialization;

namespace RobloxCS.TypeGenerator.Models;

public class RobloxApiDump {
    public required int Version { get; set; }
    public required List<RobloxClass> Classes { get; set; }

    [JsonExtensionData]
    public Dictionary<string, object> UnmappedData { get; set; } = [];
}