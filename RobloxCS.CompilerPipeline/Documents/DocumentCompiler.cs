using RobloxCS.Common;
using RobloxCS.Common.Diagnostics;
using RobloxCS.Compiler;
using RobloxCS.CompilerPipeline.Projects;
using RobloxCS.Transpiler;
using RobloxCS.Lowering;
using Document = Microsoft.CodeAnalysis.Document;

namespace RobloxCS.CompilerPipeline.Documents;

public static class DocumentCompiler {
    public static async Task<Result<CompiledFile>> CompileDocument(
        Document document,
        ProjectPlan plan,
        EmitOptions options,
        List<Diagnostic> diagnostics,
        CancellationToken cancellation
    ) {
        var syntaxTree = await document.GetSyntaxTreeAsync(cancellation);
        if (syntaxTree is null) {
            return Diagnostic.Error(DiagnosticId.NoSyntaxTree, $"Failed to get syntax tree for document '{document.Name}'.");
        }

        var model = plan.Compilation.GetSemanticModel(syntaxTree);
        var fileDiagnostics = options.SkipDiagnostics ? [] : model.GetDiagnostics(cancellationToken: cancellation);
        var compiler = new CSharpCompiler(syntaxTree, plan.Compilation, fileDiagnostics);

        diagnostics.AddRange(compiler.Diagnostics.Select(Diagnostic.FromRoslyn));

        if (compiler.HasErrors) {
            return Diagnostic.Error(DiagnosticId.SourceDidNotCompile, $"Refusing to transpile '{document.Name}', check above for compiler errors.");
        }

        var transpilerOptions = new TranspilerOptions(ScriptTypeResolver.FromFileName(document.Name), options.PrintHir) {
            Modules = options.Modules,
            RuntimeFolder = plan.RuntimeAnchor is { } runtimeAnchor
                ? $"@game/{string.Join('/', runtimeAnchor.InstancePath)}"
                : RuntimeLibrary.RequireFolder,
        };

        var code = LuauEmitter.Emit(compiler, transpilerOptions, diagnostics, out var usesRuntime);
        if (!code.Ok) {
            return code.Diagnostic;
        }

        if (usesRuntime && plan.RuntimeAnchor is null) {
            return Diagnostic.Error(
                DiagnosticId.NoRuntimeAnchor,
                $"'{document.Name}' uses the RobloxCS runtime (collections or generics), but the Rojo project doesn't map an \"include\" folder under ReplicatedStorage. Add \"rbxcs_include\": {{ \"$path\": \"include\" }} to it."
            );
        }

        var outPath = OutPathFor(document, plan);

        return new CompiledFile(outPath, code.Value) { UsesRuntime = usesRuntime };
    }
    
    public static string OutPathFor(Document document, ProjectPlan plan) {
        var filename = Path.GetFileNameWithoutExtension(document.FilePath ?? document.Name);
        var combinedOutDir = Path.Combine([plan.Anchor.FullPath, .. document.Folders]);

        return Path.Combine(combinedOutDir, $"{filename}.luau");
    }
}