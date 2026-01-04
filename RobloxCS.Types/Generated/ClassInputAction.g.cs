#nullable enable
namespace RobloxCS.Types;
[RobloxNative("InputAction", RobloxNativeType.Instance)]
public partial class InputAction : Instance  {
    public bool Enabled { get; } = default!;
    public Enums.InputActionType Type { get; } = default!;
    public void Fire() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public object GetState() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal Pressed { get; private set; } = null!;
    public RBXScriptSignal Released { get; private set; } = null!;
    public RBXScriptSignal<object> StateChanged { get; private set; } = null!;
}
