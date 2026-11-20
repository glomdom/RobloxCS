using Microsoft.CodeAnalysis.Operations;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Interpolation;

namespace RobloxCS.HIR.Builder;

public sealed partial class HirBuilder {
    private HirInterpolatedString BuildInterpolatedString(IInterpolatedStringOperation operation) {
        return new HirInterpolatedString {
            Location = operation.Syntax.GetLocation(),
            Type = operation.Type!,
            IsNotNull = true,
            Parts = [..operation.Parts.Select(BuildInterpolationPart)],
        };
    }

    private HirInterpolationPart BuildInterpolationPart(IInterpolatedStringContentOperation operation) {
        var location = operation.Syntax.GetLocation();

        return operation switch {
            IInterpolatedStringTextOperation { Text: ILiteralOperation { ConstantValue.Value: string text } } => new HirInterpolationText {
                Location = location,
                Value = text,
            },

            IInterpolationOperation { Alignment: not null } => throw new NotSupportedException("Alignment in interpolated strings is not supported yet."),
            IInterpolationOperation interpolation => new HirInterpolationExpression {
                Location = location,
                Expression = BuildExpression(interpolation.Expression),
                Format = interpolation.FormatString is ILiteralOperation { ConstantValue.Value: string format } ? format : null,
            },

            _ => throw new NotSupportedException($"Interpolated string part '{operation.Kind}' is not supported yet."),
        };
    }
}
