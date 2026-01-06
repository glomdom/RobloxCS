#nullable enable
namespace RobloxCS.Types;
[RobloxNative("MarkerCurve", RobloxNativeType.Instance)]
public partial class MarkerCurve : Instance  {
    public int Length { get; set; } = default!;
    public Dictionary<string, object> GetMarkerAtIndex() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> GetMarkers() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public List<object> InsertMarkerAtTime() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public int RemoveMarkerAtIndex() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
