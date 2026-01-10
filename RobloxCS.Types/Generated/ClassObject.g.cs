#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Object", RobloxNativeType.Instance)]
public partial class Object {
    public string ClassName { get; set; } = default!;
    public RBXScriptSignal GetPropertyChangedSignal() => ThrowHelper.ThrowTranspiledMethod();
    public bool IsA() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<string> Changed { get; private set; } = null!;
}
