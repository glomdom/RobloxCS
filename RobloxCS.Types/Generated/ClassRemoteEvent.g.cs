#nullable enable
namespace RobloxCS.Types;
[RobloxNative("RemoteEvent", RobloxNativeType.Instance)]
public partial class RemoteEvent : BaseRemoteEvent  {
    public void FireAllClients() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void FireClient() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void FireServer() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
}
