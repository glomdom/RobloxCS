#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UIPageLayout", RobloxNativeType.Instance)]
public partial class UIPageLayout : UIGridStyleLayout  {
    public bool Animated { get; set; } = default!;
    public bool Circular { get; set; } = default!;
    public GuiObject CurrentPage { get; set; } = default!;
    public Enums.EasingDirection EasingDirection { get; set; } = default!;
    public Enums.EasingStyle EasingStyle { get; set; } = default!;
    public bool GamepadInputEnabled { get; set; } = default!;
    public UDim Padding { get; set; } = default!;
    public bool ScrollWheelInputEnabled { get; set; } = default!;
    public bool TouchInputEnabled { get; set; } = default!;
    public float TweenTime { get; set; } = default!;
    public void JumpTo() => ThrowHelper.ThrowTranspiledMethod();
    public void JumpToIndex() => ThrowHelper.ThrowTranspiledMethod();
    public void Next() => ThrowHelper.ThrowTranspiledMethod();
    public void Previous() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<Instance> PageEnter { get; private set; } = null!;
    public RBXScriptSignal<Instance> PageLeave { get; private set; } = null!;
    public RBXScriptSignal<Instance> Stopped { get; private set; } = null!;
}
