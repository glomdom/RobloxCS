#nullable enable
namespace RobloxCS.Types;
[RobloxNative("JointInstance", RobloxNativeType.Instance)]
public partial class JointInstance : Instance  {
    public bool Active { get; } = default!;
    public CFrame C0 { get; } = default!;
    public CFrame C1 { get; } = default!;
    public bool Enabled { get; } = default!;
    public BasePart Part0 { get; } = default!;
    public BasePart Part1 { get; } = default!;
}
