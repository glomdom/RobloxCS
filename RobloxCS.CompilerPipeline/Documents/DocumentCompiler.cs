using RobloxCS.Common;
using RobloxCS.Common.Diagnostics;
using RobloxCS.Compiler;
using RobloxCS.CompilerPipeline.Projects;
using Document = Microsoft.CodeAnalysis.Document;

namespace RobloxCS.CompilerPipeline.Documents;

public static class DocumentCompiler {
    public static async Task<Result<CompiledFile>> CompileDocument(
        Document document,
        ProjectPlan plan,
        List<Diagnostic> diagnostics,
        CancellationToken cancellation
    ) {
        var syntaxTree = await document.GetSyntaxTreeAsync(cancellation);
        if (syntaxTree is null) return Diagnostic.Error(DiagnosticId.NoSyntaxTree, $"Failed to get syntax tree for document '{document.Name}'.");

        var compiler = new CSharpCompiler(syntaxTree, plan.Compilation);

        diagnostics.AddRange(compiler.Diagnostics.Select(Diagnostic.FromRoslyn));

        if (compiler.HasErrors) return Diagnostic.Error(DiagnosticId.SourceDidNotCompile, $"Refusing to transpile '{document.Name}', check above for compiler errors.");

        var scriptType = ScriptTypeResolver.FromFileName(document.Name);

        var code = LuauEmitter.Emit(compiler, scriptType);

        var filename = Path.GetFileNameWithoutExtension(syntaxTree.FilePath);
        var combinedOutDir = Path.Combine([plan.Anchor.FullPath, .. document.Folders]);
        var outPath = Path.Combine(combinedOutDir, $"{filename}.luau");

        return new CompiledFile(outPath, code, compiler.FormatDiagnostics());
    }
}