#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TextChatService", RobloxNativeType.Service)]
public static partial class TextChatService {
    public static bool ChatTranslationEnabled { get; } = default!;
    public static Enums.ChatVersion ChatVersion { get; } = default!;
    public static bool CreateDefaultCommands { get; } = default!;
    public static bool CreateDefaultTextChannels { get; } = default!;
    public static void DisplayBubble() => ThrowHelper.ThrowTranspiledMethod();
    public static bool CanUserChatAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static bool CanUsersChatAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static List<object> CanUsersDirectChatAsync() => ThrowHelper.ThrowTranspiledMethod();
    public static RBXScriptSignal<Instance, TextChatMessage> BubbleDisplayed { get; private set; } = null!;
    public static RBXScriptSignal<TextChatMessage> MessageReceived { get; private set; } = null!;
    public static RBXScriptSignal<TextChatMessage> SendingMessage { get; private set; } = null!;
    public static Func<TextChatMessage, Instance> OnBubbleAdded { get; set; } = default!;
    public static Func<TextChatMessage> OnChatWindowAdded { get; set; } = default!;
    public static Func<TextChatMessage> OnIncomingMessage { get; set; } = default!;
}
