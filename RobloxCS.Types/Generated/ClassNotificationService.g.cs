#nullable enable
namespace RobloxCS.Types;
[RobloxNative("NotificationService", RobloxNativeType.Service)]
public static partial class NotificationService {
    public static RBXScriptSignal<string, Enums.ConnectionState, string> Roblox17sConnectionChanged { get; private set; } = null!;
    public static RBXScriptSignal<Dictionary<object, object>> Roblox17sEventReceived { get; private set; } = null!;
}
