#nullable enable
namespace RobloxCS.Types;
[RobloxNative("UnreliableRemoteEvent", RobloxNativeType.Instance)]
public partial class UnreliableRemoteEvent : BaseRemoteEvent  {
    public void FireAllClients() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void FireClient() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public void FireServer() => throw new InvalidOperationException("Cannot call reserved method for RobloxCS transpiler.");
    public RBXScriptSignal<LuaTuple> OnClientEvent { get; private set; } = null!;
    public RBXScriptSignal<Player, LuaTuple> OnServerEvent { get; private set; } = null!;
}
