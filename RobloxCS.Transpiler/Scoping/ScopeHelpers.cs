namespace RobloxCS.Transpiler.Scoping;

public static class ScopeHelpers {
    public static IDisposable Swap<T>(ref T target, T next, Action<T> setter) {
        var prev = target;
        setter(next);

        return new Scope<T>(setter, prev);
    }
}