using RobloxCS.Common;
using RobloxCS.Common.Diagnostics;
using RobloxCS.Compiler;
using RobloxCS.Renderer;
using RobloxCS.Transpiler;

namespace RobloxCS.CompilerPipeline;

public static class LuauEmitter {
    /// <summary>
    /// Transpiles and renders one file. Transpiler diagnostics are appended to <paramref name="diagnostics"/>.
    /// </summary>
    public static Result<string> Emit(CSharpCompiler compiler, TranspilerOptions options, List<Diagnostic> diagnostics) {
        var result = new CSharpTranspiler(options, compiler).Transpile();
        diagnostics.AddRange(result.Diagnostics);

        if (!result.Ok) {
            return Diagnostic.Error(DiagnosticId.TranspilationFailed, $"Failed to transpile '{compiler.FilePath}', check above for errors.");
        }

        return new RendererWalker().Render(result.Chunk);
    }
}
