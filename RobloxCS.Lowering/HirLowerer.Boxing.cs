using Microsoft.CodeAnalysis;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.HIR.Expressions;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private Expression LowerBoxing(HirExpression operand) {
        var type = operand.Type;
        var value = LowerExpression(operand);
        if (type is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } nullable) {
            var subject = CaptureValue(value, "boxed");

            return IfExpression.From(Equal(subject, Nil()), Nil(), BoxValue(subject, nullable.TypeArguments[0], operand.Location));
        }

        return BoxValue(value, type, operand.Location);
    }

    private Expression BoxValue(Expression value, ITypeSymbol type, Location location) {
        Expression format = IsSourceEnum(type)
            ? Index(ReferenceType((INamedTypeSymbol)type, location), EnumFormatterName((INamedTypeSymbol)type))
            : Lambda([("value", Type("any"))], Type("string"), [
                Return(type.SpecialType == SpecialType.System_Boolean
                    ? IfExpression.From(Name("value"), Str("True"), Str("False"))
                    : Call("tostring", Name("value")))
            ]);

        var metadata = Table();
        metadata.Fields.Add(new NameKey { Key = "__boxedType", Value = Str(RuntimeTypeKey(type)) });
        metadata.Fields.Add(new NameKey { Key = "__isEnum", Value = Bool(type.TypeKind == TypeKind.Enum) });
        metadata.Fields.Add(new NameKey { Key = "__format", Value = format });

        var fields = Table();
        fields.Fields.Add(new NameKey { Key = "__value", Value = value });

        return Call("setmetatable", fields, Call("setmetatable", Table(), metadata));
    }

    private Expression LowerUnboxing(HirConversion conversion) {
        var target = conversion.Type;
        var nullable = target is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T };
        var underlying = nullable ? ((INamedTypeSymbol)target).TypeArguments[0] : target;

        return Cast(LowerBoxCast(LowerExpression(conversion.Operand), underlying, false, nullable), LowerType(target, conversion.Location));
    }

    private static Expression LowerBoxCast(Expression value, ITypeSymbol target, bool safe, bool allowNull = false) {
        var body = new List<Statement>();
        if (allowNull) {
            body.Add(new IfStatement { Condition = Equal(Name("value"), Nil()), Block = BlockOf([Return(Nil())]) });
        }

        body.AddRange([
            new IfStatement { Condition = NotEqual(Call("typeof", Name("value")), Str("table")), Block = BlockOf([Failure()]) },
            Local("class", Type("any"), Call("getmetatable", Name("value"))),
            new IfStatement { Condition = NotEqual(Call("typeof", Name("class")), Str("table")), Block = BlockOf([Failure()]) },
            Local("metadata", Type("any"), Call("getmetatable", Name("class"))),
            new IfStatement { Condition = NotEqual(Call("typeof", Name("metadata")), Str("table")), Block = BlockOf([Failure()]) },
            new IfStatement {
                Condition = Equal(Call("rawget", Name("metadata"), Str("__boxedType")), Name("target")),
                Block = BlockOf([Return(Call("rawget", Name("value"), Str("__value")))]),
            },
            Failure(),
        ]);

        return Call(Lambda([("value", Type("any")), ("target", Type("string"))], Type("any"), body), value, Str(RuntimeTypeKey(target)));

        ReturnStatement Failure() => Return(safe ? Nil() : Call("error", Str("Invalid unboxing cast")));
    }

    private static Expression LowerBoxReferenceCast(Expression value, bool enumOnly) {
        var body = new List<Statement> {
            new IfStatement { Condition = NotEqual(Call("typeof", Name("value")), Str("table")), Block = BlockOf([Return(Nil())]) },
            Local("class", Type("any"), Call("getmetatable", Name("value"))),
            new IfStatement { Condition = NotEqual(Call("typeof", Name("class")), Str("table")), Block = BlockOf([Return(Nil())]) },
            Local("metadata", Type("any"), Call("getmetatable", Name("class"))),
            new IfStatement { Condition = NotEqual(Call("typeof", Name("metadata")), Str("table")), Block = BlockOf([Return(Nil())]) },
            Return(IfExpression.From(enumOnly
                ? Equal(Call("rawget", Name("metadata"), Str("__isEnum")), Bool(true))
                : NotEqual(Call("rawget", Name("metadata"), Str("__boxedType")), Nil()), Name("value"), Nil())),
        };

        return Call(Lambda([("value", Type("any"))], Type("any"), body), value);
    }
}