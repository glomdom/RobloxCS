#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TextChatService", RobloxNativeType.Service)]
public static partial class TextChatService {
    public static bool ChatTranslationEnabled { get; } = default!;
    public static Enums.ChatVersion ChatVersion { get; } = default!;
    public static bool CreateDefaultCommands { get; } = default!;
    public static bool CreateDefaultTextChannels { get; } = default!;
    public static void DisplayBubble() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool CanUserChatAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static bool CanUsersChatAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public static List<object> CanUsersDirectChatAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
