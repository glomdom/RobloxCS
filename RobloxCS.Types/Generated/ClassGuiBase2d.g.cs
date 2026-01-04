#nullable enable
namespace RobloxCS.Types;
[RobloxNative("GuiBase2d", RobloxNativeType.Instance)]
public partial class GuiBase2d : GuiBase  {
    public Vector2 AbsolutePosition { get; } = default!;
    public float AbsoluteRotation { get; } = default!;
    public Vector2 AbsoluteSize { get; } = default!;
    public bool AutoLocalize { get; } = default!;
    public LocalizationTable RootLocalizationTable { get; } = default!;
    public Enums.SelectionBehavior SelectionBehaviorDown { get; } = default!;
    public Enums.SelectionBehavior SelectionBehaviorLeft { get; } = default!;
    public Enums.SelectionBehavior SelectionBehaviorRight { get; } = default!;
    public Enums.SelectionBehavior SelectionBehaviorUp { get; } = default!;
    public bool SelectionGroup { get; } = default!;
    public RBXScriptSignal<bool, GuiObject, GuiObject> SelectionChanged { get; private set; } = null!;
}
