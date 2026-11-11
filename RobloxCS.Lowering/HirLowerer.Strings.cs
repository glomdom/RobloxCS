using System.Globalization;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
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
                Expression = LowerDisplayValue(operand),
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
}
