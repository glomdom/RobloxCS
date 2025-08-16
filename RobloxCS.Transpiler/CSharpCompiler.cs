using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Serilog;

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

        Log.Information("Running diagnostics");

        var watch = Stopwatch.StartNew();
        Diagnostics = Compilation.GetDiagnostics();
        watch.Stop();

        Log.Information("Ran diagnostics for {File} in {TimeMS}ms", path, watch.ElapsedMilliseconds);
    }

    // TODO: Read from a .csproj and parse all csharp files.
    // PS: Each transpiler has it's own source tree it transpiles but it uses the same semantic model every transpiler uses.
    private SyntaxTree ParseToTree() {
        var source = File.ReadAllText(FilePath);

        return CSharpSyntaxTree.ParseText(source);
    }

    private CSharpCompilation CreateCompilation() {
        Log.Debug("Creating compilation");

        var watch = Stopwatch.StartNew();
        var references = AppDomain.CurrentDomain.GetAssemblies()
            .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location))
            .Select(a => MetadataReference.CreateFromFile(a.Location))
            .Concat([MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location)])
            .Distinct()
            .Cast<MetadataReference>()
            .ToList();

        var compilation = CSharpCompilation.Create(
            assemblyName: "Anonymous",
            syntaxTrees: [SyntaxTree],
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        watch.Stop();

        Log.Debug("Created compilation in {Time}ms", watch.ElapsedMilliseconds);

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