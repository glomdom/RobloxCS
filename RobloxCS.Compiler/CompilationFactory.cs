using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using RobloxCS.Common;

namespace RobloxCS.Compiler;

internal static class CompilationFactory {
    private const string GlobalUsingsCode = @"
        global using System;
        global using System.Collections.Generic;
        global using System.Linq;
        global using System.Text;
    ";

    public static CSharpCompilation Create(string assemblyName, SyntaxTree syntaxTree) {
        var watch = System.Diagnostics.Stopwatch.StartNew();

        var trustedAssembliesPaths = ((string)AppContext.GetData("TRUSTED_PLATFORM_ASSEMBLIES")!).Split(Path.PathSeparator);
        var references = trustedAssembliesPaths
            .Where(p => !string.IsNullOrEmpty(p))
            .Select(p => MetadataReference.CreateFromFile(p))
            .ToList();

        var globalUsingsTree = CSharpSyntaxTree.ParseText(GlobalUsingsCode);

        var compilation = CSharpCompilation.Create(
            assemblyName,
            syntaxTrees: [syntaxTree, globalUsingsTree],
            references: references,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        watch.Stop();
        Logger.Verbose("Created compilation in {Time}ms", watch.ElapsedMilliseconds);

        return compilation;
    }
}