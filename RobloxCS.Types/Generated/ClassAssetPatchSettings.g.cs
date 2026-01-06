#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AssetPatchSettings", RobloxNativeType.Instance)]
public partial class AssetPatchSettings : Instance  {
    public string ContentId { get; set; } = default!;
    public string OutputPath { get; set; } = default!;
    public string PatchId { get; set; } = default!;
}
