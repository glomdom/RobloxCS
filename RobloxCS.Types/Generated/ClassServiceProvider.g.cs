#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ServiceProvider", RobloxNativeType.Instance)]
public partial class ServiceProvider : Instance  {
    public Instance FindService() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public Instance GetService() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal Close { get; private set; } = null!;
    public RBXScriptSignal<Instance> ServiceAdded { get; private set; } = null!;
    public RBXScriptSignal<Instance> ServiceRemoving { get; private set; } = null!;
}
