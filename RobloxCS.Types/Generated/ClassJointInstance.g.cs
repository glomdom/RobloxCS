#nullable enable
namespace RobloxCS.Types;
[RobloxNative("JointInstance", RobloxNativeType.Instance)]
public partial class JointInstance : Instance  {
    public bool Active { get; set; } = default!;
    public CFrame C0 { get; set; } = default!;
    public CFrame C1 { get; set; } = default!;
    public bool Enabled { get; set; } = default!;
    public BasePart Part0 { get; set; } = default!;
    public BasePart Part1 { get; set; } = default!;
}
