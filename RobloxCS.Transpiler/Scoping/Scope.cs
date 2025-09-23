using RobloxCS.AST.Types;

namespace RobloxCS.Transpiler.Scoping;

public sealed class Scope {
    private readonly Dictionary<string, VariableSymbol> _variables = new();
    private readonly Scope? _parent;

    public Scope(Scope? parent = null) {
        _parent = parent;
    }

    public bool TryDeclare(string name, VariableSymbol symbol) => _variables.TryAdd(name, symbol);
    public VariableSymbol? Resolve(string name) => _variables.TryGetValue(name, out var symbol) ? symbol : _parent?.Resolve(name);
}

public sealed class VariableSymbol {
    public string Name { get; }
    public TypeInfo Type { get; }

    public VariableSymbol(string name, TypeInfo type) {
        Name = name;
        Type = type;
    }
}