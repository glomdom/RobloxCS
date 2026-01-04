#nullable enable
namespace RobloxCS.Types;
[RobloxNative("ScriptContext", RobloxNativeType.Service)]
public static partial class ScriptContextService {
    public static RBXScriptSignal<string, string, Instance> Error { get; private set; } = null!;
}
