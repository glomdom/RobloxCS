#nullable enable
namespace RobloxCS.Types;
[RobloxNative("InputBinding", RobloxNativeType.Instance)]
public partial class InputBinding : Instance  {
    public Enums.KeyCode Backward { get; } = default!;
    public Enums.KeyCode Down { get; } = default!;
    public Enums.KeyCode Forward { get; } = default!;
    public Enums.KeyCode KeyCode { get; } = default!;
    public Enums.KeyCode Left { get; } = default!;
    public int PointerIndex { get; } = default!;
    public float PressedThreshold { get; } = default!;
    public float ReleasedThreshold { get; } = default!;
    public float ResponseCurve { get; } = default!;
    public Enums.KeyCode Right { get; } = default!;
    public float Scale { get; } = default!;
    public GuiButton UIButton { get; } = default!;
    public Enums.KeyCode Up { get; } = default!;
    public Vector2 Vector2Scale { get; } = default!;
    public Vector3 Vector3Scale { get; } = default!;
}
