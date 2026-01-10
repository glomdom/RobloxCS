#nullable enable
namespace RobloxCS.Types;
[RobloxNative("RemoteEvent", RobloxNativeType.Instance)]
public partial class RemoteEvent : BaseRemoteEvent  {
    public void FireAllClients() => ThrowHelper.ThrowTranspiledMethod();
    public void FireClient() => ThrowHelper.ThrowTranspiledMethod();
    public void FireServer() => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<LuaTuple> OnClientEvent { get; private set; } = null!;
    public RBXScriptSignal<Player, LuaTuple> OnServerEvent { get; private set; } = null!;
}
