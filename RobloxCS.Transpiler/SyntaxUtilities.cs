using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Operations;

namespace RobloxCS.Transpiler;

public static class SyntaxUtilities {
    public static Location ResolveLocations(ImmutableArray<Location> locations) =>
        locations.Length > 1 ? throw new NotSupportedException("Partial location definitions are not supported yet.") : locations.First();

    extension(SemanticModel semantics) {
        public T CheckedGetOperation<T>(SyntaxNode node) where T : IOperation {
            var operation = semantics.GetOperation(node);

            if (operation is not T op) {
                var display = operation is null ? "null" : operation.GetType().Name;

                throw new InvalidOperationException($"Failed to get operation, '{display}' is not the expected '{typeof(T).Name}'.");
            }

            return op;
        }

        public T CheckedGetDeclaredSymbol<T>(SyntaxNode node) where T : ISymbol {
            if (semantics.GetDeclaredSymbol(node) is not T sym) {
                throw new InvalidOperationException("Failed to get symbol.");
            }

            return sym;
        }

        public T GetFirstSyntaxFromSymbol<T>(ISymbol symbol) where T : SyntaxNode {
            var syntaxRef = symbol.DeclaringSyntaxReferences.FirstOrDefault();
            if (syntaxRef?.GetSyntax() is not T syntax) {
                var node = syntaxRef?.GetSyntax();
                var display = node is null ? "null" : $"{node.Kind()} ({node.GetType().Name})";

                throw new InvalidOperationException($"Failed to get syntax, '{display}' is not the expected '{typeof(T).Name}'.");
            }

            return syntax;
        }
    }
}
