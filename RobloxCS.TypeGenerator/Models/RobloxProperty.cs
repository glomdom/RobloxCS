using System.Text.Json.Serialization;
using RobloxCS.TypeGenerator.Converters;

namespace RobloxCS.TypeGenerator.Models;

public class RobloxProperty : RobloxMember {
    public override RobloxMemberType MemberType => RobloxMemberType.Property;

    public required string Category { get; init; }

    [JsonConverter(typeof(RobloxSecurityConverter))]
    public required RobloxSecurity Security { get; init; }

    public required RobloxSerialization Serialization { get; init; }
    public List<RobloxTagKind>? Tags { get; init; }
    public RobloxThreadSafety ThreadSafety { get; init; }
    public required RobloxType ValueType { get; init; }

    public override string ToString() {
        var tagsString = Tags is not null ? $" [{string.Join(", ", Tags)}]" : string.Empty;

        return $"{Category}::{Name} [threadSafety={ThreadSafety}] [canLoad={Serialization.CanLoad} canSave={Serialization.CanSave}] [read={Security.Read}, write={Security.Write}]{tagsString}";
    }
}