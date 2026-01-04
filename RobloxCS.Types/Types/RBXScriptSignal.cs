namespace RobloxCS.Types;

public class RBXScriptSignal {
    public RBXScriptConnection Connect(Action callback) => null!;
}

public class RBXScriptSignal<T> {
    public RBXScriptConnection Connect(Action<T> callback) => null!;
}

public class RBXScriptSignal<T1, T2> {
    public RBXScriptConnection Connect(Action<T1, T2> callback) => null!;
}

public class RBXScriptSignal<T1, T2, T3> {
    public RBXScriptConnection Connect(Action<T1, T2, T3> callback) => null!;
}

public class RBXScriptSignal<T1, T2, T3, T4> {
    public RBXScriptConnection Connect(Action<T1, T2, T3, T4> callback) => null!;
}

public class RBXScriptSignal<T1, T2, T3, T4, T5> {
    public RBXScriptConnection Connect(Action<T1, T2, T3, T4, T5> callback) => null!;
}

public class RBXScriptSignal<T1, T2, T3, T4, T5, T6> {
    public RBXScriptConnection Connect(Action<T1, T2, T3, T4, T5, T6> callback) => null!;
}

public class RBXScriptSignal<T1, T2, T3, T4, T5, T6, T7> {
    public RBXScriptConnection Connect(Action<T1, T2, T3, T4, T5, T6, T7> callback) => null!;
}