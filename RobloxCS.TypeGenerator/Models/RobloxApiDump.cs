using System.Text.Json.Serialization;

namespace RobloxCS.TypeGenerator.Models;

public class RobloxApiDump {
    public required int Version { get; init; }
    public required List<RobloxClass> Classes { get; init; }

    [JsonExtensionData]
    public Dictionary<string, object> UnmappedData { get; init; } = [];
}