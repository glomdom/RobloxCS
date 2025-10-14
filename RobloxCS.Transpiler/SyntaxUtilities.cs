using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST;
using RobloxCS.AST.Types;

namespace RobloxCS.Transpiler;

public static class SyntaxUtilities {
    public static T GetSyntaxFromSymbol<T>(ISymbol symbol) where T : CSharpSyntaxNode {
        var syntaxRef = symbol.DeclaringSyntaxReferences.FirstOrDefault();
        if (syntaxRef is null) throw new Exception($"Attempted to get declaring syntax reference but was null for {symbol.Name}.");
        if (syntaxRef.GetSyntax() is not T syntax) throw new Exception($"Expected syntax to be {typeof(T).Name}, got {syntaxRef.GetSyntax().GetType().Name}");

        return syntax;
    }

    public static T? MaybeGetSyntaxFromSymbol<T>(ISymbol symbol) where T : CSharpSyntaxNode {
        var syntaxRef = symbol.DeclaringSyntaxReferences.FirstOrDefault();
        if (syntaxRef?.GetSyntax() is T syntax) return syntax;

        return null;
    }


    public static BasicTypeInfo BasicFromSymbol(ITypeSymbol symbol) {
        return symbol.SpecialType switch {
            SpecialType.System_Boolean => BasicTypeInfo.Boolean(),
            SpecialType.System_Int32 => BasicTypeInfo.Number(),
            SpecialType.System_Void => BasicTypeInfo.Void(),
            SpecialType.System_String => BasicTypeInfo.String(),

            _ => throw new ArgumentOutOfRangeException(nameof(symbol), symbol.SpecialType, null),
        };
    }

    public static string MapPrimitive(ITypeSymbol typeSymbol) {
        return typeSymbol.SpecialType switch {
            SpecialType.System_Boolean => "boolean",
            SpecialType.System_Byte => "number",
            SpecialType.System_SByte => "number",
            SpecialType.System_Int16 => "number",
            SpecialType.System_UInt16 => "number",
            SpecialType.System_Int32 => "number",
            SpecialType.System_UInt32 => "number",
            SpecialType.System_Int64 => "number",
            SpecialType.System_UInt64 => "number",
            SpecialType.System_Single => "number",
            SpecialType.System_Double => "number",
            SpecialType.System_Char => "string",
            SpecialType.System_String => "string",
            SpecialType.System_Object => "any",

            _ => throw new ArgumentOutOfRangeException(nameof(typeSymbol), typeSymbol.SpecialType, null),
        };
    }

    public static BinOp SyntaxTokenToBinOp(SyntaxToken token) {
        return token.Kind() switch {
            SyntaxKind.PlusToken => BinOp.Plus,
            SyntaxKind.MinusToken => BinOp.Minus,
            SyntaxKind.AsteriskToken => BinOp.Star,
            SyntaxKind.GreaterThanToken => BinOp.GreaterThan,
            SyntaxKind.LessThanToken => BinOp.LessThan,
            SyntaxKind.EqualsEqualsToken => BinOp.TwoEqual,
            SyntaxKind.PercentToken => BinOp.Percent,
            SyntaxKind.AmpersandAmpersandToken => BinOp.And,
            SyntaxKind.ExclamationEqualsToken => BinOp.TildeEqual,

            _ => throw new ArgumentOutOfRangeException(nameof(token), token.Kind(), null),
        };
    }

    public static UnOp SyntaxTokenToUnOp(SyntaxToken token) {
        return token.Kind() switch {
            SyntaxKind.MinusToken => UnOp.Minus,
            SyntaxKind.ExclamationToken => UnOp.Not,

            _ => throw new ArgumentOutOfRangeException(nameof(token), token.Kind(), null),
        };
    }

    public static CompoundOp SyntaxTokenToCompoundOp(SyntaxToken token) {
        return token.Kind() switch {
            SyntaxKind.PlusPlusToken => CompoundOp.PlusEqual,
            SyntaxKind.MinusMinusToken => CompoundOp.MinusEqual,
            SyntaxKind.PlusEqualsToken => CompoundOp.PlusEqual,
            SyntaxKind.MinusEqualsToken => CompoundOp.MinusEqual,

            _ => throw new ArgumentOutOfRangeException(nameof(token), token.Kind(), "Unhandled SyntaxKind in SyntaxTokenToCompoundOp"),
        };
    }

    public static ClassDeclarationSyntax? GetDeclaringClass(SyntaxNode node) {
        var current = node.Parent;
        while (current != null && current is not ClassDeclarationSyntax) {
            current = current.Parent;
        }

        return current as ClassDeclarationSyntax;
    }


    public static bool IsPrimitive(ITypeSymbol typeSymbol) {
        return typeSymbol.SpecialType switch {
            SpecialType.System_Boolean => true,
            SpecialType.System_Byte => true,
            SpecialType.System_SByte => true,
            SpecialType.System_Int16 => true,
            SpecialType.System_UInt16 => true,
            SpecialType.System_Int32 => true,
            SpecialType.System_UInt32 => true,
            SpecialType.System_Int64 => true,
            SpecialType.System_UInt64 => true,
            SpecialType.System_Single => true,
            SpecialType.System_Double => true,
            SpecialType.System_Char => true,
            SpecialType.System_String => true,
            SpecialType.System_Object => true,

            _ => false,
        };
    }

    extension(SemanticModel semantics) {
        public ITypeSymbol CheckedGetTypeInfo(TypeSyntax syntax) {
            return semantics.GetTypeInfo(syntax).Type ?? throw new InvalidOperationException($"Could not resolve type for {syntax}");
        }

        public INamedTypeSymbol CheckedGetDeclaredSymbol(BaseTypeDeclarationSyntax node) {
            var sym = semantics.GetDeclaredSymbol(node);
            if (sym is null || sym is IErrorTypeSymbol) {
                throw new Exception($"CheckedGetDeclaredSymbol failed at asking semantic model what type {node.Identifier.ValueText} is");
            }

            return sym;
        }
    }
}