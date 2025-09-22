using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace RobloxCS.Compiler;

internal static class SourceParser {
    public static SyntaxTree ParseFile(string path) {
        var source = File.ReadAllText(path);

        return CSharpSyntaxTree.ParseText(source, path: path);
    }
}