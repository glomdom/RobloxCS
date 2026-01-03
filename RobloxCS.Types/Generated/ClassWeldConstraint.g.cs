#nullable enable
namespace RobloxCS.Types;
[RobloxNative("WeldConstraint", RobloxNativeType.Instance)]
public partial class WeldConstraint : Instance  {
    public bool Active { get; } = default!;
    public bool Enabled { get; } = default!;
    public BasePart Part0 { get; } = default!;
    public BasePart Part1 { get; } = default!;
}
