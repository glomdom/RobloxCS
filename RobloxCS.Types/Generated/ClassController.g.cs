#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Controller", RobloxNativeType.Instance)]
public partial class Controller : Instance  {
    public void BindButton() => ThrowHelper.ThrowTranspiledMethod();
    public bool GetButton() => ThrowHelper.ThrowTranspiledMethod();
    public void UnbindButton() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<Enums.Button> ButtonChanged { get; private set; } = null!;
}
