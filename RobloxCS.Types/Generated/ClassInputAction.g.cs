#nullable enable
namespace RobloxCS.Types;
[RobloxNative("InputAction", RobloxNativeType.Instance)]
public partial class InputAction : Instance  {
    public bool Enabled { get; set; } = default!;
    public Enums.InputActionType Type { get; set; } = default!;
    public void Fire() => ThrowHelper.ThrowTranspiledMethod();
    public object GetState() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal Pressed { get; private set; } = null!;
    public RBXScriptSignal Released { get; private set; } = null!;
    public RBXScriptSignal<object> StateChanged { get; private set; } = null!;
}
