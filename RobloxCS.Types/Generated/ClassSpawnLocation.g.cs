#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SpawnLocation", RobloxNativeType.Instance)]
public partial class SpawnLocation : Part  {
    public bool AllowTeamChangeOnTouch { get; } = default!;
    public int Duration { get; } = default!;
    public bool Enabled { get; } = default!;
    public bool Neutral { get; } = default!;
    public BrickColor TeamColor { get; } = default!;
}
