#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ConfigSnapshot", RobloxNativeType.Instance)]
public partial class ConfigSnapshot : Object  {
    public Enums.ConfigSnapshotErrorState Error { get; set; } = default!;
    public bool Outdated { get; set; } = default!;
    public object GetValue() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal GetValueChangedSignal() => ThrowHelper.ThrowTranspiledMethod();
    public void Refresh() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal UpdateAvailable { get; private set; } = null!;
}
