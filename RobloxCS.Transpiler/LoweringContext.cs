using RobloxCS.AST.Statements;

namespace RobloxCS.Transpiler;

public sealed class LoweringContext {
    public List<Statement> Prologue { get; } = [];

    private readonly Stack<List<Statement>> _buffers = new();

    public void Emit(Statement stmt) => _buffers.Peek().Add(stmt);

    public List<Statement> Capture(Action body) {
        _buffers.Push([]);

        try {
            body();

            return _buffers.Pop();
        } catch {
            _buffers.Pop();

            throw;
        }
    }
}