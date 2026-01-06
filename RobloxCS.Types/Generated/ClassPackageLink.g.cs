#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PackageLink", RobloxNativeType.Instance)]
public partial class PackageLink : Instance  {
    public string Creator { get; set; } = default!;
    public string DefaultName { get; } = default!;
    public string PackageAssetName { get; set; } = default!;
    public string PackageId { get; set; } = default!;
    public string SerializedDefaultAttributes { get; } = default!;
    public int VersionNumber { get; } = default!;
    public Enums.PackagePermission PermissionLevel { get; set; } = default!;
}
