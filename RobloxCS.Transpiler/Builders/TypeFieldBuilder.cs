using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using RobloxCS.AST;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Types;
using Serilog;

namespace RobloxCS.Transpiler.Builders;

public static class TypeFieldBuilder {
    public static IEnumerable<TypeField> GenerateTypeFieldsFromMember(MemberDeclarationSyntax syntax, TranspilationContext ctx) {
        switch (syntax) {
            case FieldDeclarationSyntax f: {
                foreach (var tf in GenerateTypeFieldsFromField(f, ctx)) yield return tf;

                break;
            }

            case MethodDeclarationSyntax m: {
                foreach (var tf in GenerateTypeFieldsFromMethod(m, ctx)) yield return tf;

                break;
            }

            default: throw new ArgumentOutOfRangeException(nameof(syntax), syntax, null);
        }
    }

    public static IEnumerable<TypeField> GenerateTypeFieldsFromMethod(MethodDeclarationSyntax syntax, TranspilationContext ctx) {
        var symbol = ctx.Semantics.GetDeclaredSymbol(syntax);
        if (symbol is null) throw new Exception("Method declaration symbol is null.");

        if (symbol.IsStatic) throw new NotSupportedException("Static methods are not supported yet.");

        var className = symbol.ContainingType.Name;
        var classType = BasicTypeInfo.FromString($"_Instance{className}");

        var names = symbol.Parameters.Select(p => p.Name).Prepend("self").ToList();
        var types = symbol.Parameters.Select(p => SyntaxUtilities.BasicFromSymbol(p.Type)).Prepend(classType).ToList();

        var args = names.Zip(types, (name, type) => new TypeArgument { Name = name, TypeInfo = type }).ToList();

        yield return new TypeField {
            Key = NameTypeFieldKey.FromString(symbol.Name),
            Access = null,
            Value = new CallbackTypeInfo {
                Arguments = args,
                ReturnType = SyntaxUtilities.BasicFromSymbol(symbol.ReturnType),
            },
        };
    }

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

    public static IEnumerable<AssignmentStatement> CreateFieldAssignmentsFromFields(IEnumerable<IFieldSymbol> fields, TranspilationContext ctx) {
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

                yield return new AssignmentStatement {
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