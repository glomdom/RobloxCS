using System.Globalization;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.HIR.Expressions;
using Serilog;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private static Expression LowerLiteral(HirLiteral literal) {
        return literal.Value switch {
            null => Nil(),
            bool value => Bool(value),
            string value => Str(value),
            char value => Str(value.ToString()),

            _ => Num(Convert.ToDouble(literal.Value, CultureInfo.InvariantCulture)),
        };
    }

    private Expression LowerStringConcat(HirBinary binary) {
        var operands = new List<HirExpression>();
        FlattenConcat(binary, operands);

        Log.Verbose("Lowering string concatenation of {OperandCount} operands to an interpolated string", operands.Count);

        var segments = new List<InterpolatedStringSegment>();
        var text = new StringBuilder();

        foreach (var operand in operands) {
            if (operand is HirLiteral { Value: string literal }) {
                text.Append(literal);

                continue;
            }

            segments.Add(new InterpolatedStringSegment {
                Literal = text.ToString(),
                Expression = CaptureValue(LowerDisplayValue(operand), "concat"),
            });

            text.Clear();
        }

        if (segments.Count == 0) {
            return Str(text.ToString());
        }

        return new InterpolatedStringExpression {
            Segments = segments,
            LastString = text.ToString(),
        };
    }

    private static void FlattenConcat(HirExpression expression, List<HirExpression> operands) {
        while (true) {
            if (expression is HirBinary { Op: BinaryOperatorKind.Add, OperatorMethod: null, Type.SpecialType: SpecialType.System_String } binary) {
                FlattenConcat(binary.Left, operands);

                expression = binary.Right;

                continue;
            }

            operands.Add(expression);

            break;
        }
    }

    private Expression LowerDisplayValue(HirExpression expression) {
        var value = UnwrapBox(expression);
        if (IsSourceEnum(value.Type)) {
            var type = (INamedTypeSymbol)value.Type;
            if (IsFlagsEnum(type) && type.EnumUnderlyingType!.SpecialType is SpecialType.System_Int64 or SpecialType.System_UInt64) {
                Unsupported("Formatting 64-bit flags enums is not supported yet.", expression.Location);
            }

            return Call(Index(ReferenceType(type, expression.Location), EnumFormatterName(type)), LowerExpression(value));
        }

        var lowered = LowerExpression(expression);
        if (value.Type.SpecialType == SpecialType.System_Boolean) {
            return IfExpression.From(lowered, Str("True"), Str("False"));
        }

        if (value.Type.IsReferenceType || value.Type.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T) {
            var subject = Hoist(lowered, "display");
            return IfExpression.From(new BinaryOperatorExpression { Left = subject, Op = BinOp.TwoEqual, Right = Nil() }, Str(""), subject);
        }

        return lowered;
    }
}
