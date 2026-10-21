using RobloxCS.Common;
using RobloxCS.Common.Diagnostics;
using RobloxCS.Compiler;
using RobloxCS.Transpiler;

namespace RobloxCS.CompilerPipeline.Documents;

public static class FileCompiler {
    public static Result<string> Compile(string path, string typesPath, bool skipDiagnostics, EmitOptions options, List<Diagnostic> diagnostics) {
        if (!File.Exists(path)) {
            return Diagnostic.Error(DiagnosticId.FileNotFound, $"'{path}' does not exist.");
        }

        var compiler = new CSharpCompiler(path, typesPath, skipDiagnostics);
        diagnostics.AddRange(compiler.Diagnostics.Select(Diagnostic.FromRoslyn));

        if (compiler.HasErrors) {
            return Diagnostic.Error(DiagnosticId.SourceDidNotCompile, $"Refusing to transpile '{path}', C# compiler errored out.");
        }

        var transpilerOptions = new TranspilerOptions(ScriptTypeResolver.FromFileName(path), options.PrintHir);

        return LuauEmitter.Emit(compiler, transpilerOptions, diagnostics);
    }
}
