using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.HIR;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Statements;
using Serilog;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private Expression LowerBinary(HirBinary binary) {
        if (binary.Op is BinaryOperatorKind.ConditionalAnd or BinaryOperatorKind.ConditionalOr) {
            return LowerShortCircuit(binary);
        }

        if (IsSourceEnum(binary.Left.Type) && binary.Op is BinaryOperatorKind.And or BinaryOperatorKind.Or or BinaryOperatorKind.ExclusiveOr) {
            var operation = binary.Op switch {
                BinaryOperatorKind.And => "band",
                BinaryOperatorKind.Or => "bor",
                _ => "bxor",
            };

            return LowerEnumBits(binary.Left.Type, operation, [LowerExpression(binary.Left), LowerExpression(binary.Right)], binary.Location);
        }

        if (TryLowerDataTypeOperator(binary, out var dataTypeBinary)) {
            return dataTypeBinary;
        }

        if (binary.OperatorMethod is not null) {
            Unsupported($"User-defined operator '{binary.OperatorMethod.ToDisplayString()}' is not supported yet.", binary.Location);

            return Nil();
        }

        if (binary.Op is BinaryOperatorKind.Add && binary.Type.SpecialType is SpecialType.System_String) {
            return LowerStringConcat(binary);
        }

        var left = LowerExpression(binary.Left);
        var right = LowerExpression(binary.Right);

        if (binary.Op is BinaryOperatorKind.Remainder) {
            Log.Verbose("Lowering % to math.fmod to keep C#'s truncated remainder");

            return Call(Index("math", "fmod"), left, right);
        }

        if (binary.Op is BinaryOperatorKind.Divide && IsIntegral(binary.Type)) {
            Unsupported("Integer division is not supported yet.", binary.Location);

            return Nil();
        }

        if (ToBinOp(binary.Op) is not { } op) {
            Unsupported($"Binary operator '{binary.Op}' is not supported yet.", binary.Location);

            return Nil();
        }

        return new BinaryOperatorExpression {
            Left = left,
            Op = op,
            Right = right,
        };
    }

    private Expression LowerConversion(HirConversion conversion) {
        if (conversion.Kind == HirConversionKind.Numeric && (IsSourceEnum(conversion.Type) || IsSourceEnum(conversion.Operand.Type))) {
            return LowerEnumConversion(conversion);
        }

        switch (conversion.Kind) {
            case HirConversionKind.Identity:
            case HirConversionKind.Boxing:
            case HirConversionKind.Unboxing:
            case HirConversionKind.Reference:
            case HirConversionKind.NullableWrap:
            case HirConversionKind.NullableUnwrap: {
                return LowerExpression(conversion.Operand);
            }

            case HirConversionKind.Numeric: {
                if (conversion.IsImplicit) {
                    return LowerExpression(conversion.Operand);
                }

                Unsupported("Explicit numeric casts are not supported yet.", conversion.Location);

                return Nil();
            }

            default: {
                Unsupported($"'{conversion.Kind}' conversions are not supported yet.", conversion.Location);

                return Nil();
            }
        }
    }

    private static BinOp? ToBinOp(BinaryOperatorKind kind) {
        return kind switch {
            BinaryOperatorKind.Add => BinOp.Plus,
            BinaryOperatorKind.Subtract => BinOp.Minus,
            BinaryOperatorKind.Multiply => BinOp.Star,
            BinaryOperatorKind.Divide => BinOp.Slash,
            BinaryOperatorKind.Equals => BinOp.TwoEqual,
            BinaryOperatorKind.NotEquals => BinOp.TildeEqual,
            BinaryOperatorKind.LessThan => BinOp.LessThan,
            BinaryOperatorKind.LessThanOrEqual => BinOp.LessThanEqual,
            BinaryOperatorKind.GreaterThan => BinOp.GreaterThan,
            BinaryOperatorKind.GreaterThanOrEqual => BinOp.GreaterThanEqual,
            BinaryOperatorKind.ConditionalAnd => BinOp.And,
            BinaryOperatorKind.ConditionalOr => BinOp.Or,

            _ => null,
        };
    }

    private static CompoundOp? ToCompoundOp(HirCompoundAssignment compound) {
        var type = compound.Target.Type;
        var integral = IsIntegral(type);

        var numeric = compound.UserDefinedOperator is null && (integral || type.SpecialType is SpecialType.System_Single or SpecialType.System_Double);
        var dataType = compound.UserDefinedOperator is { } method && IsDataType(method.ContainingType, out _);

        if (!numeric && !dataType) {
            return null;
        }

        return compound.Op switch {
            BinaryOperatorKind.Add => CompoundOp.PlusEqual,
            BinaryOperatorKind.Subtract => CompoundOp.MinusEqual,
            BinaryOperatorKind.Multiply => CompoundOp.StarEqual,
            BinaryOperatorKind.Divide when !integral => CompoundOp.SlashEqual,

            _ => null,
        };
    }

    private static bool IsIntegral(ITypeSymbol type) {
        return type.SpecialType is SpecialType.System_SByte or SpecialType.System_Byte
            or SpecialType.System_Int16 or SpecialType.System_UInt16
            or SpecialType.System_Int32 or SpecialType.System_UInt32
            or SpecialType.System_Int64 or SpecialType.System_UInt64;
    }
}
