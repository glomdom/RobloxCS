using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Patterns;

namespace RobloxCS.HIR.Builder;

public sealed partial class HirBuilder {
    private HirIsPattern BuildTypeTest(IIsTypeOperation operation) {
        if (operation.IsNegated) {
            throw new NotSupportedException("Negated type tests are not supported.");
        }

        var location = operation.Syntax.GetLocation();

        return new HirIsPattern {
            Location = location,
            Type = operation.Type!,
            Operand = BuildExpression(operation.ValueOperand),
            Pattern = new HirTypePattern { Location = location, TestType = operation.TypeOperand },
        };
    }

    private HirIsPattern BuildIsPattern(IIsPatternOperation operation) {
        return new HirIsPattern {
            Location = operation.Syntax.GetLocation(),
            Type = operation.Type!,
            Operand = BuildExpression(operation.Value),
            Pattern = BuildPattern(operation.Pattern),
        };
    }

    private HirPattern BuildPattern(IPatternOperation operation) {
        var location = operation.Syntax.GetLocation();

        return operation switch {
            ITypePatternOperation type => new HirTypePattern { Location = location, TestType = type.MatchedType },
            IDeclarationPatternOperation { MatchesNull: true } declaration => new HirVarPattern { Location = location, Declared = declaration.DeclaredSymbol as ILocalSymbol },
            IDeclarationPatternOperation declaration => new HirTypePattern {
                Location = location,
                TestType = declaration.MatchedType!,
                Declared = declaration.DeclaredSymbol as ILocalSymbol,
            },

            IDiscardPatternOperation => new HirVarPattern { Location = location },
            IConstantPatternOperation constant => new HirConstantPattern { Location = location, Value = BuildExpression(constant.Value) },
            IRelationalPatternOperation relational => new HirRelationalPattern { Location = location, Op = relational.OperatorKind, Value = BuildExpression(relational.Value) },
            INegatedPatternOperation negated => new HirNotPattern { Location = location, Pattern = BuildPattern(negated.Pattern) },
            IBinaryPatternOperation binary => new HirBinaryPattern {
                Location = location,
                IsAnd = binary.OperatorKind == BinaryOperatorKind.And,
                Left = BuildPattern(binary.LeftPattern),
                Right = BuildPattern(binary.RightPattern),
            },

            _ => throw new NotSupportedException($"Patterns of kind '{operation.Kind}' are not supported yet."),
        };
    }
}
