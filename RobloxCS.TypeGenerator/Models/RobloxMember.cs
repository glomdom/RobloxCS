using System.Text.Json.Serialization;
using RobloxCS.TypeGenerator.Converters;

namespace RobloxCS.TypeGenerator.Models;

[JsonConverter(typeof(RobloxMemberConverter))]
public abstract class RobloxMember {
    public abstract RobloxMemberType MemberType { get; }
    public required string Name { get; init; }

    [JsonConverter(typeof(RobloxSecurityConverter))]
    public required RobloxSecurity Security { get; init; }

    public required RobloxThreadSafety ThreadSafety { get; init; }
    public List<RobloxTag>? Tags { get; init; }
}