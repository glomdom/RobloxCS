namespace RobloxCS.Types;

[RobloxNative("UnreliableRemoteEvent", RobloxNativeType.Instance)]
public class UnreliableRemoteEvent<T1> : BaseRemoteEvent {
    public void FireAllClients(T1 arg1) => ThrowHelper.ThrowTranspiledMethod();
    public void FireClient(Player player, T1 arg1) => ThrowHelper.ThrowTranspiledMethod();
    public void FireServer(T1 arg1) => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<T1> OnClientEvent { get; private set; } = null!;
    public RBXScriptSignal<Player, T1> OnServerEvent { get; private set; } = null!;
}

[RobloxNative("UnreliableRemoteEvent", RobloxNativeType.Instance)]
public class UnreliableRemoteEvent<T1, T2> : BaseRemoteEvent {
    public void FireAllClients(T1 arg1, T2 arg2) => ThrowHelper.ThrowTranspiledMethod();
    public void FireClient(Player player, T1 arg1, T2 arg2) => ThrowHelper.ThrowTranspiledMethod();
    public void FireServer(T1 arg1, T2 arg2) => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<T1, T2> OnClientEvent { get; private set; } = null!;
    public RBXScriptSignal<Player, T1, T2> OnServerEvent { get; private set; } = null!;
}

[RobloxNative("UnreliableRemoteEvent", RobloxNativeType.Instance)]
public class UnreliableRemoteEvent<T1, T2, T3> : BaseRemoteEvent {
    public void FireAllClients(T1 arg1, T2 arg2, T3 arg3) => ThrowHelper.ThrowTranspiledMethod();
    public void FireClient(Player player, T1 arg1, T2 arg2, T3 arg3) => ThrowHelper.ThrowTranspiledMethod();
    public void FireServer(T1 arg1, T2 arg2, T3 arg3) => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<T1, T2, T3> OnClientEvent { get; private set; } = null!;
    public RBXScriptSignal<Player, T1, T2, T3> OnServerEvent { get; private set; } = null!;
}

[RobloxNative("UnreliableRemoteEvent", RobloxNativeType.Instance)]
public class UnreliableRemoteEvent<T1, T2, T3, T4> : BaseRemoteEvent {
    public void FireAllClients(T1 arg1, T2 arg2, T3 arg3, T4 arg4) => ThrowHelper.ThrowTranspiledMethod();
    public void FireClient(Player player, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => ThrowHelper.ThrowTranspiledMethod();
    public void FireServer(T1 arg1, T2 arg2, T3 arg3, T4 arg4) => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<T1, T2, T3, T4> OnClientEvent { get; private set; } = null!;
    public RBXScriptSignal<Player, T1, T2, T3, T4> OnServerEvent { get; private set; } = null!;
}

[RobloxNative("UnreliableRemoteEvent", RobloxNativeType.Instance)]
public class UnreliableRemoteEvent<T1, T2, T3, T4, T5> : BaseRemoteEvent {
    public void FireAllClients(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => ThrowHelper.ThrowTranspiledMethod();
    public void FireClient(Player player, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => ThrowHelper.ThrowTranspiledMethod();
    public void FireServer(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<T1, T2, T3, T4, T5> OnClientEvent { get; private set; } = null!;
    public RBXScriptSignal<Player, T1, T2, T3, T4, T5> OnServerEvent { get; private set; } = null!;
}

[RobloxNative("UnreliableRemoteEvent", RobloxNativeType.Instance)]
public class UnreliableRemoteEvent<T1, T2, T3, T4, T5, T6> : BaseRemoteEvent {
    public void FireAllClients(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => ThrowHelper.ThrowTranspiledMethod();
    public void FireClient(Player player, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => ThrowHelper.ThrowTranspiledMethod();
    public void FireServer(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => ThrowHelper.ThrowTranspiledMethod();
    public RBXScriptSignal<T1, T2, T3, T4, T5, T6> OnClientEvent { get; private set; } = null!;
    public RBXScriptSignal<Player, T1, T2, T3, T4, T5, T6> OnServerEvent { get; private set; } = null!;
}
