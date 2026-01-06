#nullable enable
namespace RobloxCS.Types;
[RobloxNative("GroupImportData", RobloxNativeType.Instance)]
public partial class GroupImportData : BaseImportData  {
    public bool Anchored { get; set; } = default!;
    public bool ImportAsModelAsset { get; set; } = default!;
    public bool InsertInWorkspace { get; set; } = default!;
}
