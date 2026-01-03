#nullable enable
namespace RobloxCS.Types;
[RobloxNative("RotationCurve", RobloxNativeType.Instance)]
public partial class RotationCurve : Instance  {
    public int Length { get; } = default!;
    public RotationCurveKey GetKeyAtIndex() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetKeyIndicesAtTime() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetKeys() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public CFrame? GetValueAtTime() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> InsertKey() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public int RemoveKeyAtIndex() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public int SetKeys() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
