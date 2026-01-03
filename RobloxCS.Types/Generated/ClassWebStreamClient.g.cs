#nullable enable
namespace RobloxCS.Types;
[RobloxNative("WebStreamClient", RobloxNativeType.Instance)]
public partial class WebStreamClient : Object  {
    public Enums.WebStreamClientState ConnectionState { get; } = default!;
    public void Close() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void Send() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
