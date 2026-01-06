#nullable enable
namespace RobloxCS.Types;
[RobloxNative("FloatCurve", RobloxNativeType.Instance)]
public partial class FloatCurve : Instance  {
    public int Length { get; set; } = default!;
    public FloatCurveKey GetKeyAtIndex() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetKeyIndicesAtTime() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetKeys() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public float? GetValueAtTime() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> InsertKey() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public int RemoveKeyAtIndex() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public int SetKeys() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
