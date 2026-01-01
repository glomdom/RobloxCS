namespace RobloxCS.Types;

public class BindableFunction {
    public object Invoke(params object[] args) => throw new InvalidOperationException("Stub");
    public T1 Invoke<T1>(params object[] args) => throw new InvalidOperationException("Stub");
    public (T1, T2) Invoke<T1, T2>(params object[] args) => throw new InvalidOperationException("Stub");
    public (T1, T2, T3) Invoke<T1, T2, T3>(params object[] args) => throw new InvalidOperationException("Stub");
    public (T1, T2, T3, T4) Invoke<T1, T2, T3, T4>(params object[] args) => throw new InvalidOperationException("Stub");
}