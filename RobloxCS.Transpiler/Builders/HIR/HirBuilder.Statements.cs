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
            OperationKind.Invocation => HandleInvocation((IInvocationOperation)operation),
            OperationKind.FieldInitializer => HandleFieldInitializer((IFieldInitializerOperation)operation),

            _ => throw new NotSupportedException($"Operation of kind '{operation.Kind}' is not supported."),
        };

        HirAssignment HandleFieldInitializer(IFieldInitializerOperation fieldInitializerOperation) {
            if (fieldInitializerOperation.InitializedFields.Length != 1) {
                throw new InvalidOperationException("Visual Basic syntax was provided.");
            }

            var targetField = fieldInitializerOperation.InitializedFields.First();
            if (targetField.IsConst) {
                throw new NotSupportedException("Const fields are not yet supported.");
            }

            if (targetField.IsStatic) {
                throw new NotSupportedException("Static fields are not yet supported.");
            }

            var value = BuildExpression(fieldInitializerOperation.Value);
            var target = new HirFieldAccess {
                Location = fieldInitializerOperation.Syntax.GetLocation(),
                Symbol = targetField,
                Type = targetField.Type,
                Receiver = new HirThis {
                    Location = fieldInitializerOperation.Syntax.GetLocation(),
                    Type = targetField.ContainingType,
                },
            };

            return new HirAssignment {
                Location = fieldInitializerOperation.Syntax.GetLocation(),
                Target = target,
                Value = value,
            };
        }

        HirExpressionStatement HandleInvocation(IInvocationOperation invocationOperation) {
            if (invocationOperation.Type is null) {
                throw new InvalidOperationException("Invocation type cannot be null");
            }

            HirExpression? receiver = null;
            if (invocationOperation.Instance is { } inst) {
                receiver = BuildExpression(inst);
            }

            var isExtension = invocationOperation.TargetMethod.ReducedFrom is not null;

            var args = invocationOperation.Arguments.Select(BuildExpression).ToList();
            var call = new HirCall {
                Location = invocationOperation.Syntax.GetLocation(),
                Type = invocationOperation.Type,
                Method = invocationOperation.TargetMethod,
                Receiver = receiver,
                Arguments = args,
                IsExtension = isExtension,
            };

            return new HirExpressionStatement {
                Location = call.Location,
                Expression = call,
            };
        }

        HirAssignment HandleSimpleAssignment(ISimpleAssignmentOperation assignOperation) {
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