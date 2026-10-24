using RobloxCS.Common.Diagnostics;
using RobloxCS.Compiler;
using RobloxCS.HIR;
using RobloxCS.HIR.Builder;
using RobloxCS.Lowering;
using RobloxCS.Passes;

namespace RobloxCS.Transpiler;

/// <summary>
/// Runs a single file through the pipeline: Roslyn → HIR → passes → Luau AST.
/// </summary>
public sealed class CSharpTranspiler {
    public TranspilerOptions Options { get; }
    public CSharpCompiler Compiler { get; }
    public PassManager PassManager { get; } = new();

    public CSharpTranspiler(TranspilerOptions options, CSharpCompiler compiler) {
        Options = options;
        Compiler = compiler;

        if (options.PrintHir) {
            PassManager.Register(new HirPrinterPass());
        }
    }

    public TranspileResult Transpile() {
        var diagnostics = new DiagnosticBag();

        HirModule module;
        try {
            module = new HirBuilder(Compiler, diagnostics).Build();
        } catch (NotSupportedException ex) {
            // Builder bails on unsupported syntax by throwing; turn it into a diagnostic instead of crashing.
            diagnostics.Error(DiagnosticId.UnsupportedSyntax, ex.Message, null);

            return new TranspileResult(null, diagnostics.All);
        }

        if (diagnostics.HasErrors) {
            return new TranspileResult(null, diagnostics.All);
        }

        module = PassManager.Run(module, new PassContext(diagnostics));
        if (diagnostics.HasErrors) {
            return new TranspileResult(null, diagnostics.All);
        }

        var chunk = new HirLowerer(Options.ScriptType, Options.Modules, diagnostics).Lower(module);
        if (diagnostics.HasErrors) {
            return new TranspileResult(null, diagnostics.All);
        }

        return new TranspileResult(chunk, diagnostics.All);
    }
}
