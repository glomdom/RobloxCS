#nullable enable
namespace RobloxCS.Types;
[RobloxNative("WeldConstraint", RobloxNativeType.Instance)]
public partial class WeldConstraint : Instance  {
    public bool Active { get; set; } = default!;
    public bool Enabled { get; set; } = default!;
    public BasePart Part0 { get; set; } = default!;
    public BasePart Part1 { get; set; } = default!;
}
