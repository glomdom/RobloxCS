#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ReflectionMetadataClass", RobloxNativeType.Instance)]
public partial class ReflectionMetadataClass : ReflectionMetadataItem  {
    public int ExplorerImageIndex { get; set; } = default!;
    public int ExplorerOrder { get; set; } = default!;
    public bool Insertable { get; set; } = default!;
    public string PreferredParent { get; set; } = default!;
}
