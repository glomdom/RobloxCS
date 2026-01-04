namespace RobloxCS.TypeGenerator.Models;

public sealed class RobloxEvent : RobloxMember {
    public override RobloxMemberType MemberType => RobloxMemberType.Event;

    public required List<RobloxParameter> Parameters { get; init; }

    public override string ToString() {
        var tagsString = Tags is not null ? $" [{string.Join(", ", Tags)}]" : string.Empty;

        return $"::{Name}({string.Join(", ", Parameters)}) [threadSafety={ThreadSafety}] [read={Security.Read}, write={Security.Write}]{tagsString}";
    }
}