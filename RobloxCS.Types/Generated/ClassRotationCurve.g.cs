#nullable enable
namespace RobloxCS.Types;
[RobloxNative("RotationCurve", RobloxNativeType.Instance)]
public partial class RotationCurve : Instance  {
    public int Length { get; set; } = default!;
    public RotationCurveKey GetKeyAtIndex() => ThrowHelper.ThrowTranspiledMethod();
    public List<object> GetKeyIndicesAtTime() => ThrowHelper.ThrowTranspiledMethod();
    public List<object> GetKeys() => ThrowHelper.ThrowTranspiledMethod();
    public CFrame? GetValueAtTime() => ThrowHelper.ThrowTranspiledMethod();
    public List<object> InsertKey() => ThrowHelper.ThrowTranspiledMethod();
    public int RemoveKeyAtIndex() => ThrowHelper.ThrowTranspiledMethod();
    public int SetKeys() => ThrowHelper.ThrowTranspiledMethod();
}
