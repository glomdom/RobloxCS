#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PathfindingModifier", RobloxNativeType.Instance)]
public partial class PathfindingModifier : Instance  {
    public string Label { get; } = default!;
    public bool PassThrough { get; } = default!;
}
