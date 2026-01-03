#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ReflectionMetadataClass", RobloxNativeType.Instance)]
public partial class ReflectionMetadataClass : ReflectionMetadataItem  {
    public int ExplorerImageIndex { get; } = default!;
    public int ExplorerOrder { get; } = default!;
    public bool Insertable { get; } = default!;
    public string PreferredParent { get; } = default!;
}
