using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Types;
using Serilog;

namespace RobloxCS.Transpiler.Builders;

internal static class FieldBuilder {
    public static IEnumerable<TypeField> GenerateTypeFieldsFromField(FieldDeclarationSyntax fieldSyntax, TranspilationContext ctx) {
        var decl = fieldSyntax.Declaration;
        var fieldType = InferNonnull(decl.Type, ctx);
        var primitiveType = BasicTypeInfo.FromString(SyntaxUtilities.MapPrimitive(fieldType));
        var isReadonly = fieldSyntax.Modifiers.Any(SyntaxKind.ReadOnlyKeyword);

        foreach (var v in decl.Variables) {
            yield return new TypeField {
                Key = NameTypeFieldKey.FromString(v.Identifier.ValueText),
                Access = isReadonly ? AccessModifier.Read : null,
                Value = primitiveType,
            };
        }
    }

    public static IEnumerable<Assignment> CreateFieldAssignmentsFromFields(IEnumerable<IFieldSymbol> fields, TranspilationContext ctx) {
        foreach (var field in fields) {
            if (field.IsStatic) {
                Log.Warning("TODO: Implement static fields");

                continue;
            }

            foreach (var declRef in field.DeclaringSyntaxReferences) {
                if (declRef.GetSyntax() is not VariableDeclaratorSyntax v) continue;

                var init = v.Initializer;
                if (init is null) continue;

                var rhs = Lowering.ExpressionLowerer.LowerExpr(init.Value);

                yield return new Assignment {
                    Vars = [VarName.FromString($"self.{field.Name}")],
                    Expressions = [rhs],
                };
            }
        }
    }

    private static ITypeSymbol InferNonnull(TypeSyntax syntax, TranspilationContext ctx) {
        var fieldType = ctx.Semantics.GetTypeInfo(syntax).Type!;
        if (fieldType is IErrorTypeSymbol or null) {
            throw new Exception("Error occured while attempting to infer type.");
        }

        Log.Verbose("Inferred type {Type} from {Name}", syntax.ToString(), fieldType.Name);

        return fieldType;
    }
}