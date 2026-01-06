#nullable enable
namespace RobloxCS.Types;
[RobloxNative("CompositeValueCurve", RobloxNativeType.Instance)]
public partial class CompositeValueCurve : Instance  {
    public Enums.CompositeValueCurveType CurveType { get; set; } = default!;
    public List<Instance> GetComponentCurves() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public object GetValueAtTime() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
