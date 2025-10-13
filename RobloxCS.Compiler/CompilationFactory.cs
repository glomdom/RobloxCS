using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RobloxCS.Common;

namespace RobloxCS.Compiler;

internal static class CompilationFactory {
    public static CSharpCompilation Create(string assemblyName, SyntaxTree syntaxTree) {
        var watch = System.Diagnostics.Stopwatch.StartNew();

        var references = AppDomain.CurrentDomain
            .GetAssemblies()
            .Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location))
            .Select(a => MetadataReference.CreateFromFile(a.Location))
            .Append(MetadataReference.CreateFromFile(typeof(List<>).Assembly.Location))
            .Distinct()
            .ToList();

        var compilation = CSharpCompilation.Create(
            assemblyName: assemblyName,
            syntaxTrees: new[] { syntaxTree },
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        watch.Stop();
        Logger.Verbose("Created compilation in {Time}ms", watch.ElapsedMilliseconds);

        return compilation;
    }
}