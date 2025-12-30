using System.Text.Json.Serialization;
using RobloxCS.TypeGenerator.Converters;

namespace RobloxCS.TypeGenerator.Models;

[JsonConverter(typeof(RobloxMemberConverter))]
public abstract class RobloxMember {
    public abstract RobloxMemberType MemberType { get; }
    public required string Name { get; set; }
    
    [JsonExtensionData]
    public Dictionary<string, object> UnmappedData { get; set; } = [];
}