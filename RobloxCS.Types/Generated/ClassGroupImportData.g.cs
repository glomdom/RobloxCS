#nullable enable
namespace RobloxCS.Types;
[RobloxNative("GroupImportData", RobloxNativeType.Instance)]
public partial class GroupImportData : BaseImportData  {
    public bool Anchored { get; } = default!;
    public bool ImportAsModelAsset { get; } = default!;
    public bool InsertInWorkspace { get; } = default!;
}
