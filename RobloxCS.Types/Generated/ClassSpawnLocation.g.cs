#nullable enable
namespace RobloxCS.Types;
[RobloxNative("SpawnLocation", RobloxNativeType.Instance)]
public partial class SpawnLocation : Part  {
    public bool AllowTeamChangeOnTouch { get; set; } = default!;
    public int Duration { get; set; } = default!;
    public bool Enabled { get; set; } = default!;
    public bool Neutral { get; set; } = default!;
    public BrickColor TeamColor { get; set; } = default!;
}
