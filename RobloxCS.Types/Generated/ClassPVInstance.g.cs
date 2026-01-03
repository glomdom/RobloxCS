#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PVInstance", RobloxNativeType.Instance)]
public partial class PVInstance : Instance  {
    public CFrame Origin { get; } = default!;
    public CFrame PivotOffset { get; } = default!;
    public CFrame GetPivot() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void PivotTo() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
