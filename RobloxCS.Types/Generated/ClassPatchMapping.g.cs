#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PatchMapping", RobloxNativeType.Instance)]
public partial class PatchMapping : Instance  {
    public bool FlattenTree { get; } = default!;
    public string PatchId { get; } = default!;
    public string TargetPath { get; } = default!;
}
