using System.Text.Json.Serialization;

namespace RobloxCS.TypeGenerator.Models;

public class RobloxApiDump {
    public required int Version { get; init; }
    public required List<RobloxClass> Classes { get; init; }
    public required List<RobloxEnum> Enums { get; init; }

    [JsonExtensionData]
    public Dictionary<string, object> UnmappedData { get; init; } = [];
}