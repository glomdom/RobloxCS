#nullable enable
namespace RobloxCS.Types;
[RobloxNative("WebSocketClient", RobloxNativeType.Instance)]
public partial class WebSocketClient : Instance  {
    public Enums.WebSocketState ConnectionState { get; } = default!;
    public void Close() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Send() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
