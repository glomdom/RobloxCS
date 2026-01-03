#nullable enable
namespace RobloxCS.Types;
[RobloxNative("TextChannel", RobloxNativeType.Instance)]
public partial class TextChannel : Instance  {
    public Player DirectChatRequester { get; } = default!;
    public TextChatMessage DisplaySystemMessage() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void SetDirectChatRequester() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public LuaTuple AddUserAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public TextChatMessage SendAsync() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
