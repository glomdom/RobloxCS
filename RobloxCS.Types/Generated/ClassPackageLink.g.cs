#nullable enable
namespace RobloxCS.Types;
[RobloxNative("PackageLink", RobloxNativeType.Instance)]
public partial class PackageLink : Instance  {
    public string Creator { get; } = default!;
    public string DefaultName { get; } = default!;
    public string PackageAssetName { get; } = default!;
    public string PackageId { get; } = default!;
    public string SerializedDefaultAttributes { get; } = default!;
    public int VersionNumber { get; } = default!;
    public Enums.PackagePermission PermissionLevel { get; } = default!;
}
