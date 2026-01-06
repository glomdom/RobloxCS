#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PatchMapping", RobloxNativeType.Instance)]
public partial class PatchMapping : Instance  {
    public bool FlattenTree { get; set; } = default!;
    public string PatchId { get; set; } = default!;
    public string TargetPath { get; set; } = default!;
}
