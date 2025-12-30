using System.Text.Json.Serialization;

namespace RobloxCS.TypeGenerator.Models;

public class RobloxClass {
    public required string Name { get; set; }
    public required List<RobloxMember> Members { get; set; }

    [JsonExtensionData]
    public Dictionary<string, object> UnmappedData { get; set; } = [];
}