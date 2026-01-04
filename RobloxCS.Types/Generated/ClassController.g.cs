#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Controller", RobloxNativeType.Instance)]
public partial class Controller : Instance  {
    public void BindButton() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public bool GetButton() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void UnbindButton() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<Enums.Button> ButtonChanged { get; private set; } = null!;
}
