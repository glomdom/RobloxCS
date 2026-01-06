#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ProximityPrompt", RobloxNativeType.Instance)]
public partial class ProximityPrompt : Instance  {
    public string ActionText { get; set; } = default!;
    public bool AutoLocalize { get; set; } = default!;
    public bool ClickablePrompt { get; set; } = default!;
    public bool Enabled { get; set; } = default!;
    public Enums.ProximityPromptExclusivity Exclusivity { get; set; } = default!;
    public Enums.KeyCode GamepadKeyCode { get; set; } = default!;
    public float HoldDuration { get; set; } = default!;
    public Enums.KeyCode KeyboardKeyCode { get; set; } = default!;
    public float MaxActivationDistance { get; set; } = default!;
    public float MaxIndicatorDistance { get; set; } = default!;
    public string ObjectText { get; set; } = default!;
    public bool RequiresLineOfSight { get; set; } = default!;
    public LocalizationTable RootLocalizationTable { get; set; } = default!;
    public Enums.ProximityPromptStyle Style { get; set; } = default!;
    public Vector2 UIOffset { get; set; } = default!;
    public void InputHoldBegin() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void InputHoldEnd() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal IndicatorHidden { get; private set; } = null!;
    public RBXScriptSignal IndicatorShown { get; private set; } = null!;
    public RBXScriptSignal<Player> PromptButtonHoldBegan { get; private set; } = null!;
    public RBXScriptSignal<Player> PromptButtonHoldEnded { get; private set; } = null!;
    public RBXScriptSignal PromptHidden { get; private set; } = null!;
    public RBXScriptSignal<Enums.ProximityPromptInputType> PromptShown { get; private set; } = null!;
    public RBXScriptSignal<Player> TriggerEnded { get; private set; } = null!;
    public RBXScriptSignal<Player> Triggered { get; private set; } = null!;
}
