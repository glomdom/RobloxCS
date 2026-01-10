using System.Diagnostics;
using Basic.Reference.Assemblies;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Serilog;

namespace RobloxCS.Compiler;

internal static class CompilationFactory {
    private const string GlobalUsingsCode = @"
        global using System;
        global using System.Collections.Generic;
        global using System.Linq;
        global using System.Text;
    ";

    private const string TypesRef = @"RobloxCS.Types\bin\Debug\net10.0\RobloxCS.Types.dll";

    public static CSharpCompilation Create(string assemblyName, SyntaxTree syntaxTree) {
        var watch = Stopwatch.StartNew();

        var standardRefs = Net100.References.All;
        var allRefs = standardRefs.Append(MetadataReference.CreateFromFile(TypesRef));

        var globalUsingsTree = CSharpSyntaxTree.ParseText(GlobalUsingsCode);

        var compilation = CSharpCompilation.Create(
            assemblyName,
            syntaxTrees: [syntaxTree, globalUsingsTree],
            references: allRefs,
            options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
        );

        watch.Stop();
        Log.Verbose("Created compilation in {Time}ms", watch.ElapsedMilliseconds);

        return compilation;
    }
}