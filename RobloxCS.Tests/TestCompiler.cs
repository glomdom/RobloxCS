using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RobloxCS.Compiler;
using RobloxCS.Lowering;
using RobloxCS.Modules;
using RobloxCS.Transpiler;

namespace RobloxCS.Tests;

internal static class TestCompiler {
    private static readonly Lazy<MetadataReference[]> References = new(() => {
        var path = Path.Join(TestContext.CurrentContext.TestDirectory, "Data", "Enums.client.cs");
        return [..new CSharpCompiler(path, "RobloxCS.Types.dll", true).Compilation.References];
    });

    public static SyntaxTree Parse(string source, string path = "Test.cs") => CSharpSyntaxTree.ParseText(source, new CSharpParseOptions(LanguageVersion.Preview), path);

    public static CSharpCompilation Compile(params SyntaxTree[] trees) =>
        CSharpCompilation.Create("CompilerTests", trees, References.Value, new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

    public static TranspileResult Transpile(SyntaxTree tree, CSharpCompilation compilation, ScriptType scriptType = ScriptType.Module, ModuleGraph? modules = null) =>
        new CSharpTranspiler(new TranspilerOptions(scriptType) { Modules = modules ?? ModuleGraph.Empty },
            new CSharpCompiler(tree, compilation, [..compilation.GetDiagnostics()])
        ).Transpile();
}