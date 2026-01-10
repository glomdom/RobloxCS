using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Operations;
using RobloxCS.AST;
using RobloxCS.AST.Types;
using RobloxCS.Types;
using Serilog;
using TypeInfo = RobloxCS.AST.Types.TypeInfo;

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

    // cursed
    public static T ExtractAttributeFromAttributes<T>(ImmutableArray<AttributeData> attributes) where T : Attribute {
        var targetType = typeof(T);
        var targetName = targetType.FullName;

        var attrData = attributes.FirstOrDefault(ad => ad.AttributeClass?.ToDisplayString() == targetName);
        if (attrData is null) {
            throw new Exception($"Failed to find attribute {targetName} when searching inside attributes.");
        }

        var ctor = targetType.GetConstructors().First(ci => ci.GetParameters().Length == attrData.ConstructorArguments.Length);
        var ctorArgs = ctor.GetParameters()
            .Zip(attrData.ConstructorArguments, (pi, tc) => pi.ParameterType.IsEnum ? Enum.ToObject(pi.ParameterType, tc.Value!) : tc.Value).ToArray();

        var instance = (T)Activator.CreateInstance(targetType, ctorArgs)!;

        foreach (var (argName, argVal) in attrData.NamedArguments) {
            var info = targetType.GetProperty(argName);
            if (info is null || !info.CanWrite) continue;

            var val = info.PropertyType.IsEnum ? Enum.ToObject(info.PropertyType, argVal.Value!) : argVal.Value;
            info.SetValue(instance, val);
        }

        return instance;
    }

    public static TypeInfo TypeInfoFromSymbol(ITypeSymbol symbol, TranspilationContext ctx) {
        return symbol.SpecialType switch {
            SpecialType.System_Boolean => BasicTypeInfo.Boolean(),
            SpecialType.System_Int32 => BasicTypeInfo.Number(),
            SpecialType.System_Void => BasicTypeInfo.Void(),
            SpecialType.System_String => BasicTypeInfo.String(),
            SpecialType.None => GetTypeFromNonPrimitive(symbol, ctx),

            _ => throw new ArgumentOutOfRangeException(nameof(symbol), symbol.SpecialType, null),
        };
    }

    private static TypeInfo GetTypeFromNonPrimitive(ITypeSymbol symbol, TranspilationContext ctx) {
        Log.Verbose(
            "Getting type info for {SymbolName} ({SymbolTypeKind} / {SymbolSpecialType}) from assembly {ContainingAssembly}",
            symbol.Name, symbol.TypeKind,
            symbol.SpecialType, symbol.ContainingAssembly.Name
        );

        if (symbol.TypeKind is TypeKind.Class or TypeKind.Struct) {
            if (symbol is INamedTypeSymbol namedSymbol && IsSystemList(namedSymbol, ctx)) {
                return GetTypeFromList(namedSymbol, ctx);
            }

            var classNs = symbol.ContainingNamespace;
            var isRobloxType = classNs is not null && classNs.ToDisplayString() == "RobloxCS.Types";

            if (isRobloxType) {
                var nativeAttribute = ExtractAttributeFromAttributes<RobloxNativeAttribute>(symbol.GetAttributes());

                return BasicTypeInfo.FromString(nativeAttribute.RobloxName);
            }

            return BasicTypeInfo.FromString($"_Instance{symbol.Name}");
        }

        throw new NotSupportedException($"Unsupported non-primitive type {symbol} for {symbol.Name}");
    }

    /// <summary>
    /// Gets the element type of the provided List symbol.
    /// </summary>
    public static ArrayTypeInfo GetTypeFromList(INamedTypeSymbol symbol, TranspilationContext ctx) {
        return new ArrayTypeInfo { ElementType = TypeInfoFromSymbol(symbol.TypeArguments.First(), ctx) };
    }

    public static bool IsSystemList(INamedTypeSymbol symbol, TranspilationContext ctx) {
        if (symbol.Name != "List" || symbol.Arity != 1) return false;

        return SymbolEqualityComparer.Default.Equals(symbol.OriginalDefinition, ctx.Compiler.Types.ListTypeSymbol.OriginalDefinition);
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

    public static bool IsInvokedOnExternalObject(
        InvocationExpressionSyntax invocation,
        SemanticModel semanticModel,
        [NotNullWhen(true)] out IOperation? external
    ) {
        external = null;

        if (semanticModel.GetOperation(invocation) is not IInvocationOperation operation) return false;
        if (operation.TargetMethod.IsStatic) return false;

        var receiver = operation.Instance;
        if (receiver is IInstanceReferenceOperation) return false;

        external = receiver!;

        return true;
    }

    extension(SemanticModel semantics) {
        public ITypeSymbol CheckedGetTypeInfo(TypeSyntax syntax) {
            return semantics.GetTypeInfo(syntax).ConvertedType ?? throw new InvalidOperationException($"Could not resolve type for {syntax}");
        }

        public INamedTypeSymbol CheckedGetDeclaredSymbol(BaseTypeDeclarationSyntax node) {
            var sym = semantics.GetDeclaredSymbol(node);
            if (sym is null || sym is IErrorTypeSymbol) {
                throw new Exception($"CheckedGetDeclaredSymbol failed at asking semantic model what type {node.Identifier.ValueText} is");
            }

            return sym;
        }

        public T GetSymbol<T>(CSharpSyntaxNode node) where T : ISymbol {
            var symbol = semantics.GetSymbolInfo(node).Symbol;
            if (symbol is null) {
                throw new Exception("Attempted to get symbol from MemberAccessExpressionSyntax but got null.");
            }

            return (T)symbol;
        }

        public ISymbol GetSymbol(CSharpSyntaxNode node) {
            var symbol = semantics.GetSymbolInfo(node).Symbol;

            return symbol ?? throw new Exception("Attempted to get symbol from MemberAccessExpressionSyntax but got null.");
        }
    }
}