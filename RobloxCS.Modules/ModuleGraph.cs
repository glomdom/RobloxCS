using Microsoft.CodeAnalysis;

namespace RobloxCS.Modules;

public sealed record ModuleLocation(IReadOnlyList<string> InstancePath, bool IsScript);

public sealed class ModuleGraph {
    public static ModuleGraph Empty { get; } = new(new Dictionary<string, ModuleLocation>());

    private readonly IReadOnlyDictionary<string, ModuleLocation> _types;

    public ModuleGraph(IReadOnlyDictionary<string, ModuleLocation> types) {
        _types = types;
    }

    public bool TryGetModule(INamedTypeSymbol type, out ModuleLocation location) {
        return _types.TryGetValue(KeyOf(type), out location!);
    }

    public static string KeyOf(INamedTypeSymbol type) {
        return type.OriginalDefinition.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
    }
}