namespace RobloxCS.TypeGenerator.Models;

public class RobloxProperty : RobloxMember {
    public override RobloxMemberType MemberType => RobloxMemberType.Property;

    public required string Category { get; init; }

    public required RobloxSerialization Serialization { get; init; }
    public required RobloxType ValueType { get; init; }

    public override string ToString() {
        var tagsString = Tags is not null ? $" [{string.Join(", ", Tags)}]" : string.Empty;

        return
            $"{Category}::{Name} [threadSafety={ThreadSafety}] [canLoad={Serialization.CanLoad} canSave={Serialization.CanSave}] [read={Security.Read}, write={Security.Write}]{tagsString}";
    }
}