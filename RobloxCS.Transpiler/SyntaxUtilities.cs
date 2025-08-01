using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace RobloxCS.Transpiler;

public static class SyntaxUtilities {
    public static bool ShouldBeExported(ClassDeclarationSyntax decl) {
        var modifiers = decl.Modifiers;
        var hasPublic = modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword));

        return hasPublic;
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
            
            _ => throw new ArgumentOutOfRangeException(nameof(typeSymbol), typeSymbol.SpecialType, null)
        };
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

            _ => false
        };
    }
}