using System.Diagnostics;
using RobloxCS.AST;
using Serilog;

namespace RobloxCS.Transpiler.Scoping;

public sealed class Scope : IDisposable {
    public Block Value { get; }
    public HashSet<string> Locals { get; } = new();
    public uint NextTempN { get; private set; }

    private readonly string? _name;
    private readonly Stack<Block> _stack;

    public Scope(Stack<Block> stack, Block value, string? name = null) {
        _name = name;
        _stack = stack;
        _stack.Push(value);

        Value = value;

        Log.Debug("Pushed scope {Name}", GetFriendlyName());
    }

    public bool AddLocal(string name) => Locals.Add(name);
    public bool HasLocal(string name) => Locals.Contains(name);
    public string NewTempName(string prefix = "_tmp_") => $"{prefix}{NextTempN++}";

    public void Dispose() {
        var popped = _stack.Pop();

        Log.Debug("Popped block scope");
        Debug.Assert(ReferenceEquals(popped, Value), "Scope stack imbalance, please run compiler with verbose input and file an issue.");
    }

    public string GetFriendlyName() {
        return _name ?? "unnamed";
    }
}