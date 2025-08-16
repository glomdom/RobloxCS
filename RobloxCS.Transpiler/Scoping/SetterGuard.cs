using Serilog;

namespace RobloxCS.Transpiler.Scoping;

public sealed class SetterGuard<T> : IDisposable {
    private readonly Action<T> _setter;
    private readonly T _prev;

    public SetterGuard(Action<T> setter, T prev, T next) {
        _setter = setter;
        _prev = prev;

        _setter(next);
        
        Log.Debug("Pushed setter guard prev={Prev} next={Next}", prev, next);
    }

    public void Dispose() {
        _setter(_prev);
        
        Log.Debug("Popped setter guard");
    }
}