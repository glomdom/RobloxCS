#nullable enable
namespace RobloxCS.Types;
[RobloxNative("EulerRotationCurve", RobloxNativeType.Instance)]
public partial class EulerRotationCurve : Instance  {
    public Enums.RotationOrder RotationOrder { get; } = default!;
    public List<object> GetAnglesAtTime() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public CFrame GetRotationAtTime() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public FloatCurve X() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public FloatCurve Y() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public FloatCurve Z() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
