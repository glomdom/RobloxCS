using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RobloxCS.Transpiler;

public sealed class CSharpCompiler {
    public SyntaxTree SyntaxTree { get; }
    public CSharpCompilation Compilation { get; }
    public ImmutableArray<Diagnostic> Diagnostics { get; }

    public CompilationUnitSyntax Root => SyntaxTree.GetCompilationUnitRoot();
    public string FilePath { get; }

    public CSharpCompiler(string path) {
        FilePath = path;

        SyntaxTree = ParseToTree();
        Compilation = CreateCompilation();
        Diagnostics = Compilation.GetDiagnostics();
    }

    // TODO: Read from a .csproj and parse all csharp files.
    // PS: Each transpiler has it's own source tree it transpiles but it uses the same semantic model every transpiler uses.
    private SyntaxTree ParseToTree() {
        var source = File.ReadAllText(FilePath);

        return CSharpSyntaxTree.ParseText(source);
    }

    private CSharpCompilation CreateCompilation() {
        var references = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location))
            .Select(a => MetadataReference.CreateFromFile(a.Location))
            .Cast<MetadataReference>();

        var compilation = CSharpCompilation.Create(
            assemblyName: "Anonymous",
            syntaxTrees: [SyntaxTree],
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        return compilation;
    }

    public ImmutableArray<string> FormatDiagnostics() {
        var diagArray = new List<string>();

        foreach (var diagnostic in Diagnostics) {
            var span = $"{diagnostic.Location.SourceSpan.Start},{diagnostic.Location.SourceSpan.End}";
            var fullPath = Path.GetFullPath(FilePath);

            if (diagnostic is { Severity: DiagnosticSeverity.Warning, IsWarningAsError: false }) {
                diagArray.Add($"{fullPath}({span}): [bold yellow]warning {diagnostic.Id}[/] : {diagnostic.GetMessage()}");
            } else if (
                diagnostic is { Severity: DiagnosticSeverity.Warning, IsWarningAsError: true } ||
                diagnostic.Severity == DiagnosticSeverity.Error
            ) {
                diagArray.Add($"{fullPath}({span}): [bold red]error {diagnostic.Id}[/] : {diagnostic.GetMessage()}");
            }
        }

        return [..diagArray];
    }
}