#nullable enable
namespace RobloxCS.Types;
[RobloxNative("BindableEvent", RobloxNativeType.Instance)]
public partial class BindableEvent : Instance  {
    public void Fire() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<LuaTuple> Event { get; private set; } = null!;
}
