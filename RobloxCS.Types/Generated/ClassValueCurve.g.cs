#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ValueCurve", RobloxNativeType.Instance)]
public partial class ValueCurve : Instance  {
    public int Length { get; set; } = default!;
    public string ValueType { get; set; } = default!;
    public ValueCurveKey GetKeyAtIndex() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetKeyIndicesAtTime() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetKeys() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public object? GetValueAtTime() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> InsertKey() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> InsertKeyValue() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public int RemoveKeyAtIndex() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public int SetKeys() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
