using System.Text.Json.Serialization;

namespace RobloxCS.TypeGenerator.Models;

public class RobloxClass {
    public required string Name { get; init; }
    public required List<RobloxMember> Members { get; init; }

    [JsonExtensionData]
    public Dictionary<string, object> UnmappedData { get; init; } = [];
}