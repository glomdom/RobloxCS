using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Statements;

namespace RobloxCS.Transpiler.Builders.HIR;

public sealed partial class HirBuilder {
    public HirStatement BuildStatement(IOperation operation) {
        return operation.Kind switch {
            OperationKind.VariableDeclarationGroup => HandleVariableDeclarationGroup((IVariableDeclarationGroupOperation)operation),
            OperationKind.ExpressionStatement => HandleExpressionStatement((IExpressionStatementOperation)operation),
            OperationKind.SimpleAssignment => HandleSimpleAssignment((ISimpleAssignmentOperation)operation),

            _ => throw new NotSupportedException($"Operation of kind '{operation.Kind}' is not supported."),
        };

        HirStatement HandleSimpleAssignment(ISimpleAssignmentOperation assignOperation) {
            if (assignOperation.IsRef) {
                throw new NotSupportedException("Ref assignments are not supported.");
            }

            var target = BuildExpression(assignOperation.Target);
            var value = BuildExpression(assignOperation.Value);

            return new HirAssignment {
                Location = assignOperation.Syntax.GetLocation(),
                Target = target,
                Value = value,
            };
        }

        HirStatement HandleExpressionStatement(IExpressionStatementOperation stmtOperation) {
            return BuildStatement(stmtOperation.Operation); // this might backfire in the future
        }

        HirLocalDeclaration HandleVariableDeclarationGroup(IVariableDeclarationGroupOperation declOperation) {
            var declarators = new List<HirVariableDeclarator>();

            foreach (var decl in declOperation.Declarations) {
                declarators.AddRange(decl.Declarators.Select(HandleVariableDeclarator));
            }

            return new HirLocalDeclaration {
                Location = operation.Syntax.GetLocation(),
                Declarators = declarators,
            };
        }

        HirVariableDeclarator HandleVariableDeclarator(IVariableDeclaratorOperation declOperation) {
            HirExpression? initializer = null;
            if (declOperation.Initializer is { } init) {
                initializer = BuildExpression(init);
            }

            return new HirVariableDeclarator {
                Location = declOperation.Syntax.GetLocation(),
                Symbol = declOperation.Symbol,
                Initializer = initializer,
            };
        }
    }
}