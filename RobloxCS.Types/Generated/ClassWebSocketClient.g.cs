#nullable enable
namespace RobloxCS.Types;
[RobloxNative("WebSocketClient", RobloxNativeType.Instance)]
public partial class WebSocketClient : Instance  {
    public Enums.WebSocketState ConnectionState { get; } = default!;
    public void Close() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Send() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal Closed { get; private set; } = null!;
    public RBXScriptSignal<string> MessageReceived { get; private set; } = null!;
    public RBXScriptSignal Opened { get; private set; } = null!;
}
