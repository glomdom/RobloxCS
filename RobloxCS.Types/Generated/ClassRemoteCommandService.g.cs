#nullable enable
namespace RobloxCS.Types;
[RobloxNative("RemoteCommandService", RobloxNativeType.Service)]
public static partial class RemoteCommandService {
    public static Player GetExecutingPlayer() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static RBXScriptSignal GetReceivedUpdateSignal() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static RBXScriptSignal GetStoppingSignal() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void SendUpdate() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
