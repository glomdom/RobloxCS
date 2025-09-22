using System.Collections.Immutable;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Serilog;

namespace RobloxCS.Compiler;

public sealed class CSharpCompiler {
    public string FilePath { get; }
    public SyntaxTree SyntaxTree { get; }
    public CSharpCompilation Compilation { get; }
    public ImmutableArray<Diagnostic> Diagnostics { get; }

    public CompilationUnitSyntax Root => SyntaxTree.GetCompilationUnitRoot();

    public CSharpCompiler(string path) {
        FilePath = path;

        SyntaxTree = SourceParser.ParseFile(path);
        Compilation = CompilationFactory.Create("Anonymous", SyntaxTree);

        Log.Information("Running diagnostics for {File}", path);
        var watch = Stopwatch.StartNew();

        Diagnostics = Compilation.GetDiagnostics();

        watch.Stop();
        Log.Information("Ran diagnostics for {File} in {TimeMS}ms", path, watch.ElapsedMilliseconds);
    }

    public ImmutableArray<string> FormatDiagnostics() {
        var formatter = new DiagnosticFormatter();

        return [..Diagnostics.Select(d => formatter.Format(d))];
    }
}