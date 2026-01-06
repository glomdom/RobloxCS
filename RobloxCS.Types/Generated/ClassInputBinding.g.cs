#nullable enable
namespace RobloxCS.Types;
[RobloxNative("InputBinding", RobloxNativeType.Instance)]
public partial class InputBinding : Instance  {
    public Enums.KeyCode Backward { get; set; } = default!;
    public Enums.KeyCode Down { get; set; } = default!;
    public Enums.KeyCode Forward { get; set; } = default!;
    public Enums.KeyCode KeyCode { get; set; } = default!;
    public Enums.KeyCode Left { get; set; } = default!;
    public int PointerIndex { get; set; } = default!;
    public float PressedThreshold { get; set; } = default!;
    public float ReleasedThreshold { get; set; } = default!;
    public float ResponseCurve { get; set; } = default!;
    public Enums.KeyCode Right { get; set; } = default!;
    public float Scale { get; set; } = default!;
    public GuiButton UIButton { get; set; } = default!;
    public Enums.KeyCode Up { get; set; } = default!;
    public Vector2 Vector2Scale { get; set; } = default!;
    public Vector3 Vector3Scale { get; set; } = default!;
}
