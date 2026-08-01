using RobloxCS.Common;
using RobloxCS.Common.Diagnostics;
using RobloxCS.Compiler;

namespace RobloxCS.CompilerPipeline.Documents;

public static class FileCompiler {
    public static Result<string> Compile(string path, string typesPath, bool skipDiagnostics, List<Diagnostic> diagnostics) {
        if (!File.Exists(path)) {
            return Diagnostic.Error(DiagnosticId.FileNotFound, $"'{path}' does not exist.");
        }

        var compiler = new CSharpCompiler(path, typesPath, skipDiagnostics);
        diagnostics.AddRange(compiler.Diagnostics.Select(Diagnostic.FromRoslyn));

        if (compiler.HasErrors) {
            return Diagnostic.Error(DiagnosticId.SourceDidNotCompile, $"Refusing to transpile '{path}', C# compiler errored out.");
        }

        return LuauEmitter.Emit(compiler, ScriptTypeResolver.FromFileName(path));
    }
}