using System.Text.Json.Serialization;
using RobloxCS.TypeGenerator.Converters;

namespace RobloxCS.TypeGenerator.Models;

public class RobloxFunction : RobloxMember {
    public override RobloxMemberType MemberType => RobloxMemberType.Function;

    public required List<RobloxFunctionParameter> Parameters { get; set; }
    public required RobloxType ReturnType { get; set; }

    [JsonConverter(typeof(RobloxSecurityConverter))]
    public required RobloxSecurity Security { get; set; }

    public required RobloxThreadSafety ThreadSafety { get; set; }
    public List<RobloxTagKind>? Tags { get; set; }

    public override string ToString() {
        var tagsString = Tags is not null ? $" [{string.Join(", ", Tags)}]" : string.Empty;
        
        return $"{Name}({string.Join(", ", Parameters)}) -> {ReturnType.Name} [read={Security.Read}, write={Security.Write}]{tagsString}";
    }
}