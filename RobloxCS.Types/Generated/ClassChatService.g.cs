#nullable enable
namespace RobloxCS.Types;
[RobloxNative("Chat", RobloxNativeType.Service)]
public static partial class ChatService {
    public static bool BubbleChatEnabled { get; } = default!;
    public static bool LoadDefaultChat { get; } = default!;
    public static void Chat() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static LuaTuple InvokeChatCallback() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void RegisterChatCallback() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static void SetBubbleChatSettings() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool CanUserChatAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool CanUsersChatAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static string FilterStringAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static string FilterStringForBroadcast() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
