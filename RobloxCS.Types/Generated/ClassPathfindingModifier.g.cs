#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PathfindingModifier", RobloxNativeType.Instance)]
public partial class PathfindingModifier : Instance  {
    public string Label { get; set; } = default!;
    public bool PassThrough { get; set; } = default!;
}
