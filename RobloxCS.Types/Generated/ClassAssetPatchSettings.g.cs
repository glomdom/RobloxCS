#nullable enable
namespace RobloxCS.Types;
[RobloxNative("AssetPatchSettings", RobloxNativeType.Instance)]
public partial class AssetPatchSettings : Instance  {
    public string ContentId { get; } = default!;
    public string OutputPath { get; } = default!;
    public string PatchId { get; } = default!;
}
