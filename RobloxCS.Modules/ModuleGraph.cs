using Microsoft.CodeAnalysis;

namespace RobloxCS.Modules;

public sealed record ModuleLocation(IReadOnlyList<string> InstancePath, bool IsScript);

public sealed class ModuleGraph {
    public static ModuleGraph Empty { get; } = new(new Dictionary<string, ModuleLocation>());
    public IReadOnlyDictionary<string, ModuleLocation> Types { get; }

    public ModuleGraph(IReadOnlyDictionary<string, ModuleLocation> types) {
        Types = types;
    }

    public bool TryGetModule(INamedTypeSymbol type, out ModuleLocation location) {
        return Types.TryGetValue(KeyOf(type), out location!);
    }

    public static string KeyOf(INamedTypeSymbol type) {
        return type.OriginalDefinition.ToDisplayString(SymbolDisplayFormat.FullyQualifiedFormat);
    }
}