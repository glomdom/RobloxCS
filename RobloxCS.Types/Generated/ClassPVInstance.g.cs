#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PVInstance", RobloxNativeType.Instance)]
public partial class PVInstance : Instance  {
    public CFrame Origin { get; set; } = default!;
    public CFrame PivotOffset { get; set; } = default!;
    public CFrame GetPivot() => ThrowHelper.ThrowTranspiledMethod();
    public void PivotTo() => ThrowHelper.ThrowTranspiledMethod();
}
