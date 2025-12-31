using System.Text.Json.Serialization;
using RobloxCS.TypeGenerator.Converters;

namespace RobloxCS.TypeGenerator.Models;

public sealed class RobloxCallback : RobloxMember {
    public override RobloxMemberType MemberType => RobloxMemberType.Callback;

    public required List<RobloxParameter> Parameters { get; init; }
    public required RobloxType ReturnType { get; init; }

    [JsonConverter(typeof(RobloxSecurityConverter))]
    public required RobloxSecurity Security { get; init; }

    public required RobloxThreadSafety ThreadSafety { get; init; }
    public List<RobloxTagKind>? Tags { get; init; }

    public override string ToString() {
        var tagsString = Tags is not null ? $" [{string.Join(", ", Tags)}]" : string.Empty;

        return $"{Name}({string.Join(", ", Parameters)}) -> {ReturnType.Name} [threadSafety={ThreadSafety}] [read={Security.Read}, write={Security.Write}]{tagsString}";
    }
}