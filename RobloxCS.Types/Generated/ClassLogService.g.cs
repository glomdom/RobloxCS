#nullable enable
namespace RobloxCS.Types;
[RobloxNative("LogService", RobloxNativeType.Service)]
public static partial class LogService {
    public static void ClearOutput() => ThrowHelper.ThrowTranspiledMethod();
    public static List<object> GetLogHistory() => ThrowHelper.ThrowTranspiledMethod();
    public static RBXScriptSignal<string, Enums.MessageType> MessageOut { get; private set; } = null!;
}
