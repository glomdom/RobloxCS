#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ConfigSnapshot", RobloxNativeType.Instance)]
public partial class ConfigSnapshot : Object  {
    public Enums.ConfigSnapshotErrorState Error { get; set; } = default!;
    public bool Outdated { get; set; } = default!;
    public object GetValue() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal GetValueChangedSignal() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Refresh() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal UpdateAvailable { get; private set; } = null!;
}
