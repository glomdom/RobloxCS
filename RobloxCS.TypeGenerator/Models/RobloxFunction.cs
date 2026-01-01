using System.Text.Json.Serialization;
using RobloxCS.TypeGenerator.Converters;

namespace RobloxCS.TypeGenerator.Models;

public class RobloxFunction : RobloxMember {
    public override RobloxMemberType MemberType => RobloxMemberType.Function;

    public required List<RobloxParameter> Parameters { get; init; }
    public required RobloxType ReturnType { get; init; }

    public override string ToString() {
        var tagsString = Tags is not null ? $" [{string.Join(", ", Tags)}]" : string.Empty;
        
        return $"{Name}({string.Join(", ", Parameters)}) -> {ReturnType.Name} [threadSafety={ThreadSafety}] [read={Security.Read}, write={Security.Write}]{tagsString}";
    }
}