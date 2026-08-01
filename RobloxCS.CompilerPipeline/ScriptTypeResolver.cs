using RobloxCS.Transpiler;

namespace RobloxCS.CompilerPipeline;

public static class ScriptTypeResolver {
    public static ScriptType FromFileName(string path) {
        var name = Path.GetFileName(path);

        return name switch {
            _ when name.EndsWith(".server.cs", StringComparison.OrdinalIgnoreCase) => ScriptType.Server,
            _ when name.EndsWith(".client.cs", StringComparison.OrdinalIgnoreCase) => ScriptType.Local,

            _ => ScriptType.Module,
        };
    }
}