#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Dialog", RobloxNativeType.Instance)]
public partial class Dialog : Instance  {
    public Enums.DialogBehaviorType BehaviorType { get; } = default!;
    public float ConversationDistance { get; } = default!;
    public bool GoodbyeChoiceActive { get; } = default!;
    public string GoodbyeDialog { get; } = default!;
    public bool InUse { get; } = default!;
    public string InitialPrompt { get; } = default!;
    public Enums.DialogPurpose Purpose { get; } = default!;
    public Enums.DialogTone Tone { get; } = default!;
    public float TriggerDistance { get; } = default!;
    public Vector3 TriggerOffset { get; } = default!;
    public List<Instance> GetCurrentPlayers() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
