#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Object", RobloxNativeType.Instance)]
public partial class Object {
    public string ClassName { get; } = default!;
    public RBXScriptSignal GetPropertyChangedSignal() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public bool IsA() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
