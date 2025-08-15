namespace RobloxCS.Transpiler.Scoping;

public struct Scope<T>(Action<T> setter, T previous) : IDisposable {
    public void Dispose() => setter(previous);
}