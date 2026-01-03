#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Team", RobloxNativeType.Instance)]
public partial class Team : Instance  {
    public bool AutoAssignable { get; } = default!;
    public BrickColor TeamColor { get; } = default!;
    public List<Instance> GetPlayers() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
