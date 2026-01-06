#nullable enable
namespace RobloxCS.Types;
[RobloxNative("CharacterMesh", RobloxNativeType.Instance)]
public partial class CharacterMesh : CharacterAppearance  {
    public int BaseTextureId { get; set; } = default!;
    public Enums.BodyPart BodyPart { get; set; } = default!;
    public int MeshId { get; set; } = default!;
    public int OverlayTextureId { get; set; } = default!;
}
