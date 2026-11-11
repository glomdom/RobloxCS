using System.Globalization;
using Microsoft.CodeAnalysis;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.HIR.Declarations;
using RobloxCS.HIR.Expressions;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private bool IsSourceEnum(ITypeSymbol type) => type.TypeKind == TypeKind.Enum && !IsRobloxEnum(type) && IsTranspiledType(type);

    private List<Statement> LowerEnum(HirType type) {
        if (type.Symbol.ContainingType is not null) {
            Unsupported("Nested enums are not supported yet.", type.Location);

            return [];
        }

        var table = Table();
        foreach (var field in type.Fields) {
            var value = Convert.ToDecimal(field.Symbol.ConstantValue, CultureInfo.InvariantCulture);
            if (value < -9_007_199_254_740_991m || value > 9_007_199_254_740_991m) {
                Unsupported("Enum values must be within Luau's exact integer range (53 bits).", field.Location);

                continue;
            }

            table.Fields.Add(new NameKey { Key = field.Name, Value = Num((double)value) });
        }

        var name = TypeName(type.Symbol);
        return [Local(name, table), TypeDecl(name, Type("number"), true), LowerEnumFormatter(type)];
    }

    private Expression LowerEnumBits(ITypeSymbol type, string operation, List<Expression> arguments, Location location) {
        if (type is not INamedTypeSymbol { EnumUnderlyingType: { } underlying } || underlying.SpecialType is SpecialType.System_Int64 or SpecialType.System_UInt64) {
            Unsupported("Bitwise operations on 64-bit enums are not supported yet.", location);

            return Nil();
        }

        return NormalizeEnumInteger(Call(Index("bit32", operation), arguments), underlying);
    }

    private Expression LowerEnumConversion(HirConversion conversion) {
        var target = conversion.Type is INamedTypeSymbol { EnumUnderlyingType: { } underlying } ? underlying : conversion.Type;
        var value = LowerExpression(conversion.Operand);

        if (target.SpecialType is SpecialType.System_Single or SpecialType.System_Double or SpecialType.System_Decimal) {
            return value;
        }

        if (conversion.Operand.Type.SpecialType is SpecialType.System_Single or SpecialType.System_Double or SpecialType.System_Decimal) {
            var operand = Hoist(value, "enumValue");
            value = IfExpression.From(
                new BinaryOperatorExpression { Left = operand, Op = BinOp.LessThan, Right = Num(0) },
                Call(Index("math", "ceil"), operand),
                Call(Index("math", "floor"), operand)
            );
        }

        return NormalizeEnumInteger(value, target);
    }

    private Expression NormalizeEnumInteger(Expression value, ITypeSymbol underlying) {
        var (bits, signed) = underlying.SpecialType switch {
            SpecialType.System_SByte => (8, true),
            SpecialType.System_Byte => (8, false),
            SpecialType.System_Int16 => (16, true),
            SpecialType.System_UInt16 or SpecialType.System_Char => (16, false),
            SpecialType.System_Int32 => (32, true),
            SpecialType.System_UInt32 => (32, false),
            _ => (64, true),
        };

        if (bits == 64) {
            return value;
        }

        var modulus = Math.Pow(2, bits);
        if (value is NumberExpression literal) {
            var normalized = (literal.Value % modulus + modulus) % modulus;
            return Num(signed && normalized >= modulus / 2 ? normalized - modulus : normalized);
        }

        var wrapped = new BinaryOperatorExpression { Left = value, Op = BinOp.Percent, Right = Num(modulus) };
        if (!signed) {
            return wrapped;
        }

        var result = Hoist(wrapped, "enumValue");
        return IfExpression.From(
            new BinaryOperatorExpression { Left = result, Op = BinOp.GreaterThanEqual, Right = Num(modulus / 2) },
            new BinaryOperatorExpression { Left = result, Op = BinOp.Minus, Right = Num(modulus) },
            result
        );
    }
}
