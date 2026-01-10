#nullable enable
namespace RobloxCS.Types;
[RobloxNative("CompositeValueCurve", RobloxNativeType.Instance)]
public partial class CompositeValueCurve : Instance  {
    public Enums.CompositeValueCurveType CurveType { get; set; } = default!;
    public List<Instance> GetComponentCurves() => ThrowHelper.ThrowTranspiledMethod();
    public object GetValueAtTime() => ThrowHelper.ThrowTranspiledMethod();
}
