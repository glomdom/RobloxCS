#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UIPageLayout", RobloxNativeType.Instance)]
public partial class UIPageLayout : UIGridStyleLayout  {
    public bool Animated { get; } = default!;
    public bool Circular { get; } = default!;
    public GuiObject CurrentPage { get; } = default!;
    public Enums.EasingDirection EasingDirection { get; } = default!;
    public Enums.EasingStyle EasingStyle { get; } = default!;
    public bool GamepadInputEnabled { get; } = default!;
    public UDim Padding { get; } = default!;
    public bool ScrollWheelInputEnabled { get; } = default!;
    public bool TouchInputEnabled { get; } = default!;
    public float TweenTime { get; } = default!;
    public void JumpTo() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void JumpToIndex() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Next() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Previous() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
