#nullable enable
namespace RobloxCS.Types;
[RobloxNative("CharacterMesh", RobloxNativeType.Instance)]
public partial class CharacterMesh : CharacterAppearance  {
    public int BaseTextureId { get; } = default!;
    public Enums.BodyPart BodyPart { get; } = default!;
    public int MeshId { get; } = default!;
    public int OverlayTextureId { get; } = default!;
}
