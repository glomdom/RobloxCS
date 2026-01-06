#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Team", RobloxNativeType.Instance)]
public partial class Team : Instance  {
    public bool AutoAssignable { get; set; } = default!;
    public BrickColor TeamColor { get; set; } = default!;
    public List<Instance> GetPlayers() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<Player> PlayerAdded { get; private set; } = null!;
    public RBXScriptSignal<Player> PlayerRemoved { get; private set; } = null!;
}
