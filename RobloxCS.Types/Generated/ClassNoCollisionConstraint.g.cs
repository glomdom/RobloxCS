#nullable enable
namespace RobloxCS.Types;
[RobloxNative("NoCollisionConstraint", RobloxNativeType.Instance)]
public partial class NoCollisionConstraint : Instance  {
    public bool Enabled { get; } = default!;
    public BasePart Part0 { get; } = default!;
    public BasePart Part1 { get; } = default!;
}
