#nullable enable
namespace RobloxCS.Types;
[RobloxNative("WebStreamClient", RobloxNativeType.Instance)]
public partial class WebStreamClient : Object  {
    public Enums.WebStreamClientState ConnectionState { get; set; } = default!;
    public void Close() => ThrowHelper.ThrowTranspiledMethod();
    public void Send() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal Closed { get; private set; } = null!;
    public RBXScriptSignal<int, string> Error { get; private set; } = null!;
    public RBXScriptSignal<string> MessageReceived { get; private set; } = null!;
    public RBXScriptSignal<int, string> Opened { get; private set; } = null!;
}
