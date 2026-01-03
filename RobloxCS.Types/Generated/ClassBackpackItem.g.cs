#nullable enable
namespace RobloxCS.Types;
[RobloxNative("BackpackItem", RobloxNativeType.Instance)]
public partial class BackpackItem : Model  {
    public string TextureContent { get; } = default!;
    public string TextureId { get; } = default!;
}
