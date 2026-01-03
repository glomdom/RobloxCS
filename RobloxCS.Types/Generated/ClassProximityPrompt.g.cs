#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ProximityPrompt", RobloxNativeType.Instance)]
public partial class ProximityPrompt : Instance  {
    public string ActionText { get; } = default!;
    public bool AutoLocalize { get; } = default!;
    public bool ClickablePrompt { get; } = default!;
    public bool Enabled { get; } = default!;
    public Enums.ProximityPromptExclusivity Exclusivity { get; } = default!;
    public Enums.KeyCode GamepadKeyCode { get; } = default!;
    public float HoldDuration { get; } = default!;
    public Enums.KeyCode KeyboardKeyCode { get; } = default!;
    public float MaxActivationDistance { get; } = default!;
    public float MaxIndicatorDistance { get; } = default!;
    public string ObjectText { get; } = default!;
    public bool RequiresLineOfSight { get; } = default!;
    public LocalizationTable RootLocalizationTable { get; } = default!;
    public Enums.ProximityPromptStyle Style { get; } = default!;
    public Vector2 UIOffset { get; } = default!;
    public void InputHoldBegin() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void InputHoldEnd() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
