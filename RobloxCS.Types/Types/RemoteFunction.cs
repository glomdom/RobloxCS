namespace RobloxCS.Types;

[RobloxNative("RemoteFunction", RobloxNativeType.Instance)]
public class RemoteFunction<TResult> : Instance {
    public TResult InvokeClient(Player player) => ThrowHelper.ThrowTranspiledMethod();
    public TResult InvokeServer() => ThrowHelper.ThrowTranspiledMethod();
    public Func<TResult> OnClientInvoke { get; set; } = null!;
    public Func<Player, TResult> OnServerInvoke { get; set; } = null!;
}

[RobloxNative("RemoteFunction", RobloxNativeType.Instance)]
public class RemoteFunction<T1, TResult> : Instance {
    public TResult InvokeClient(Player player, T1 arg1) => ThrowHelper.ThrowTranspiledMethod();
    public TResult InvokeServer(T1 arg1) => ThrowHelper.ThrowTranspiledMethod();
    public Func<T1, TResult> OnClientInvoke { get; set; } = null!;
    public Func<Player, T1, TResult> OnServerInvoke { get; set; } = null!;
}

[RobloxNative("RemoteFunction", RobloxNativeType.Instance)]
public class RemoteFunction<T1, T2, TResult> : Instance {
    public TResult InvokeClient(Player player, T1 arg1, T2 arg2) => ThrowHelper.ThrowTranspiledMethod();
    public TResult InvokeServer(T1 arg1, T2 arg2) => ThrowHelper.ThrowTranspiledMethod();
    public Func<T1, T2, TResult> OnClientInvoke { get; set; } = null!;
    public Func<Player, T1, T2, TResult> OnServerInvoke { get; set; } = null!;
}

[RobloxNative("RemoteFunction", RobloxNativeType.Instance)]
public class RemoteFunction<T1, T2, T3, TResult> : Instance {
    public TResult InvokeClient(Player player, T1 arg1, T2 arg2, T3 arg3) => ThrowHelper.ThrowTranspiledMethod();
    public TResult InvokeServer(T1 arg1, T2 arg2, T3 arg3) => ThrowHelper.ThrowTranspiledMethod();
    public Func<T1, T2, T3, TResult> OnClientInvoke { get; set; } = null!;
    public Func<Player, T1, T2, T3, TResult> OnServerInvoke { get; set; } = null!;
}

[RobloxNative("RemoteFunction", RobloxNativeType.Instance)]
public class RemoteFunction<T1, T2, T3, T4, TResult> : Instance {
    public TResult InvokeClient(Player player, T1 arg1, T2 arg2, T3 arg3, T4 arg4) => ThrowHelper.ThrowTranspiledMethod();
    public TResult InvokeServer(T1 arg1, T2 arg2, T3 arg3, T4 arg4) => ThrowHelper.ThrowTranspiledMethod();
    public Func<T1, T2, T3, T4, TResult> OnClientInvoke { get; set; } = null!;
    public Func<Player, T1, T2, T3, T4, TResult> OnServerInvoke { get; set; } = null!;
}

[RobloxNative("RemoteFunction", RobloxNativeType.Instance)]
public class RemoteFunction<T1, T2, T3, T4, T5, TResult> : Instance {
    public TResult InvokeClient(Player player, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => ThrowHelper.ThrowTranspiledMethod();
    public TResult InvokeServer(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5) => ThrowHelper.ThrowTranspiledMethod();
    public Func<T1, T2, T3, T4, T5, TResult> OnClientInvoke { get; set; } = null!;
    public Func<Player, T1, T2, T3, T4, T5, TResult> OnServerInvoke { get; set; } = null!;
}

[RobloxNative("RemoteFunction", RobloxNativeType.Instance)]
public class RemoteFunction<T1, T2, T3, T4, T5, T6, TResult> : Instance {
    public TResult InvokeClient(Player player, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => ThrowHelper.ThrowTranspiledMethod();
    public TResult InvokeServer(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6) => ThrowHelper.ThrowTranspiledMethod();
    public Func<T1, T2, T3, T4, T5, T6, TResult> OnClientInvoke { get; set; } = null!;
    public Func<Player, T1, T2, T3, T4, T5, T6, TResult> OnServerInvoke { get; set; } = null!;
}
