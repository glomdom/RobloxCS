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

    public MetadataTypes Types => field ??= new MetadataTypes(Compilation); // lazy on purpose, do not change.

    public bool HasErrors => Diagnostics.Any(d => d.Severity == DiagnosticSeverity.Error);

    public CompilationUnitSyntax Root => SyntaxTree.GetCompilationUnitRoot();

    public CSharpCompiler(string path, string typesPath, bool skipDiagnostics) {
        FilePath = path;

        SyntaxTree = SourceParser.ParseFile(path);
        Compilation = CompilationFactory.Create("Anonymous", SyntaxTree, typesPath);

        EnsureHasReferences(Compilation, path);

        if (!skipDiagnostics) {
            Log.Information("Running diagnostics for {File}", path);
            var watch = Stopwatch.StartNew();

            Diagnostics = [.. Compilation.GetDiagnostics().Where(d => d.Severity != DiagnosticSeverity.Hidden)];

            watch.Stop();
            Log.Information("Ran diagnostics for {File} in {TimeMS}ms", path, watch.ElapsedMilliseconds);
        } else {
            Diagnostics = [];
        }
    }

    public CSharpCompiler(SyntaxTree tree, Compilation compilation) {
        SyntaxTree = tree;
        FilePath = tree.FilePath;
        Compilation = (CSharpCompilation)compilation;
        Diagnostics = [.. Compilation.GetDiagnostics().Where(d => d.Severity != DiagnosticSeverity.Hidden)];

        EnsureHasReferences(Compilation, tree.FilePath);
    }

    private static void EnsureHasReferences(CSharpCompilation compilation, string context) {
        if (compilation.References.Any()) return;

        throw new InvalidOperationException(
            $"Compilation for '{context}' has zero metadata references, so nothing in the BCL " +
            "will resolve. MSBuildWorkspace does not run a restore -- if obj/ or " +
            "project.assets.json is missing from the target project, run `dotnet restore` on it " +
            "and try again."
        );
    }

    public ImmutableArray<string> FormatDiagnostics() {
        var formatter = new DiagnosticFormatter();

        return [.. Diagnostics.Select(d => formatter.Format(d))];
    }

    public sealed class MetadataTypes {
        public readonly INamedTypeSymbol GenericListTypeSymbol;

        public MetadataTypes(CSharpCompilation compilation) {
            GenericListTypeSymbol = CheckedGetType(compilation, "System.Collections.Generic.List`1");
        }

        private static INamedTypeSymbol CheckedGetType(CSharpCompilation compilation, string name) {
            var candidates = compilation.GetTypesByMetadataName(name);

            return candidates.Length switch {
                1 => candidates[0],

                0 => throw new InvalidOperationException(
                    $"'{name}' was not found across {compilation.References.Count()} metadata " +
                    "reference(s). If the count is 0 the project failed to restore; otherwise the " +
                    "reference set is missing the assembly that declares it."
                ),

                _ => throw new InvalidOperationException(
                    $"'{name}' is ambiguous across: " +
                    string.Join(", ", candidates.Select(c => c.ContainingAssembly.Identity)) +
                    ". Two referenced assemblies declare the same type; drop one from the " +
                    "reference set."
                ),
            };
        }
    }
}