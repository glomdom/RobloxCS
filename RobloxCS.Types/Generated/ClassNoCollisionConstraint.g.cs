#nullable enable
namespace RobloxCS.Types;
[RobloxNative("NoCollisionConstraint", RobloxNativeType.Instance)]
public partial class NoCollisionConstraint : Instance  {
    public bool Enabled { get; set; } = default!;
    public BasePart Part0 { get; set; } = default!;
    public BasePart Part1 { get; set; } = default!;
}
