#nullable enable
namespace RobloxCS.Types;
[RobloxNative("BodyColors", RobloxNativeType.Instance)]
public partial class BodyColors : CharacterAppearance  {
    public BrickColor HeadColor { get; } = default!;
    public Color3 HeadColor3 { get; } = default!;
    public BrickColor LeftArmColor { get; } = default!;
    public Color3 LeftArmColor3 { get; } = default!;
    public BrickColor LeftLegColor { get; } = default!;
    public Color3 LeftLegColor3 { get; } = default!;
    public BrickColor RightArmColor { get; } = default!;
    public Color3 RightArmColor3 { get; } = default!;
    public BrickColor RightLegColor { get; } = default!;
    public Color3 RightLegColor3 { get; } = default!;
    public BrickColor TorsoColor { get; } = default!;
    public Color3 TorsoColor3 { get; } = default!;
}
