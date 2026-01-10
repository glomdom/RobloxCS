#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ServiceProvider", RobloxNativeType.Instance)]
public partial class ServiceProvider : Instance  {
    public Instance FindService() => ThrowHelper.ThrowTranspiledMethod();
    public Instance GetService() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal Close { get; private set; } = null!;
    public RBXScriptSignal<Instance> ServiceAdded { get; private set; } = null!;
    public RBXScriptSignal<Instance> ServiceRemoving { get; private set; } = null!;
}
