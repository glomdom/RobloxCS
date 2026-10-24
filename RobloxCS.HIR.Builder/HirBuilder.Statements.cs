using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Statements;

namespace RobloxCS.HIR.Builder;

public sealed partial class HirBuilder {
    public HirStatement BuildStatement(IOperation operation) {
        return operation.Kind switch {
            OperationKind.VariableDeclarationGroup => HandleVariableDeclarationGroup((IVariableDeclarationGroupOperation)operation),
            OperationKind.ExpressionStatement => HandleExpressionStatement((IExpressionStatementOperation)operation),
            OperationKind.SimpleAssignment => HandleSimpleAssignment((ISimpleAssignmentOperation)operation),
            OperationKind.Invocation => HandleInvocation((IInvocationOperation)operation),
            OperationKind.FieldInitializer => HandleFieldInitializer((IFieldInitializerOperation)operation),
            OperationKind.Increment or OperationKind.Decrement => HandleIncrementDecrement((IIncrementOrDecrementOperation)operation),
            OperationKind.Loop => HandleLoop((ILoopOperation)operation),
            OperationKind.Conditional => HandleConditional((IConditionalOperation)operation),
            OperationKind.Branch => HandleBranch((IBranchOperation)operation),
            OperationKind.Return => HandleReturn((IReturnOperation)operation),

            _ => throw new NotSupportedException($"Statement operation of kind '{operation.Kind}' is not supported."),
        };

        HirStatement HandleReturn(IReturnOperation returnOperation) {
            var expr = returnOperation.ReturnedValue is not null ? BuildExpression(returnOperation.ReturnedValue) : null;
            
            return new HirReturn {
                Location = returnOperation.Syntax.GetLocation(),
                Value = expr,
            };
        }

        HirStatement HandleBranch(IBranchOperation branchOperation) {
            return branchOperation.BranchKind switch {
                BranchKind.Continue => new HirContinue {
                    Location = branchOperation.Syntax.GetLocation(),
                },
                
                BranchKind.Break => new HirBreak {
                    Location = branchOperation.Syntax.GetLocation(),
                },
                
                _ => throw new NotSupportedException($"Branch operation of kind '{branchOperation.BranchKind}' is not supported."),
            };
        }

        HirStatement HandleConditional(IConditionalOperation conditionalOperation) {
            var cond = BuildExpression(conditionalOperation.Condition);
            var thenExpr = BuildBlock(conditionalOperation.WhenTrue);
            var elseExpr = conditionalOperation.WhenFalse is not null ? BuildStatement(conditionalOperation.WhenFalse) : null;
            
            return new HirIf {
                Location = conditionalOperation.Syntax.GetLocation(),
                Condition = cond,
                Then = thenExpr,
                Else = elseExpr,
            };
        }

        HirStatement HandleLoop(ILoopOperation loopOperation) {
            return loopOperation.LoopKind switch {
                LoopKind.For => HandleForLoop((IForLoopOperation)loopOperation),
                LoopKind.While => HandleWhileLoop((IWhileLoopOperation)loopOperation),
                
                _ => throw new NotSupportedException($"Loop operation of kind '{loopOperation.LoopKind}' is not supported."),
            };
        }
        
        HirStatement HandleWhileLoop(IWhileLoopOperation loop) {
            if (loop.Condition is not { } condOperation) {
                throw new InvalidOperationException("While loop operation does not have a condition.");
            }
            
            var cond = BuildExpression(condOperation);
            var block = BuildBlock(loop.Body);
            
            return new HirWhile {
                Location = loop.Syntax.GetLocation(),
                Condition = cond,
                Body = block,
            };
        }
        
        HirStatement HandleForLoop(IForLoopOperation loop) {
            var inits = loop.Before.Select(BuildStatement).ToImmutableArray();
            var incrementors = loop.AtLoopBottom.Select(BuildStatement).ToImmutableArray();
            var cond = loop.Condition is not null ? BuildExpression(loop.Condition) : null;
            var block = BuildBlock(loop.Body);
            
            return new HirFor {
                Location = loop.Syntax.GetLocation(),
                Initializers = inits,
                Condition = cond,
                Incrementors = incrementors,
                Body = block,
            };
        }

        HirStatement HandleIncrementDecrement(IIncrementOrDecrementOperation incrementOrDecrementOperation) {
            if (incrementOrDecrementOperation.Type is not { } type) {
                throw new InvalidOperationException("Increment or decrement operation does not produce a type.");
            }

            var dec = new HirIncrementDecrement {
                Location = incrementOrDecrementOperation.Syntax.GetLocation(),
                Type = type,
                Target = BuildExpression(incrementOrDecrementOperation.Target),
                IsPrefix = !incrementOrDecrementOperation.IsPostfix,
                IsIncrement = incrementOrDecrementOperation.Kind == OperationKind.Increment,
            };

            return new HirExpressionStatement {
                Location = incrementOrDecrementOperation.Syntax.GetLocation(),
                Expression = dec,
            };
        }

        HirAssignment HandleFieldInitializer(IFieldInitializerOperation fieldInitializerOperation) {
            if (fieldInitializerOperation.InitializedFields.Length != 1) {
                throw new InvalidOperationException("Visual Basic syntax was provided.");
            }

            var targetField = fieldInitializerOperation.InitializedFields.First();
            if (targetField.IsConst) {
                throw new NotSupportedException("Const field initializers are not yet supported.");
            }

            if (targetField.IsStatic) {
                throw new NotSupportedException("Static field initializers are not yet supported.");
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

            var args = invocationOperation.Arguments.Select(BuildArgument).ToList();
            var call = new HirCall {
                Location = invocationOperation.Syntax.GetLocation(),
                Type = invocationOperation.Type,
                Method = invocationOperation.TargetMethod,
                Receiver = receiver,
                Arguments = [.. args],
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
                Declarators = [.. declarators],
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
    
    private HirBlock BuildBlock(IOperation operation) {
        if (operation is IBlockOperation block) {
            return new HirBlock {
                Location = block.Syntax.GetLocation(),
                Statements = [.. block.Operations.Select(BuildStatement)],
                Locals = [.. block.Locals],
            };
        }

        return new HirBlock {
            Location = operation.Syntax.GetLocation(),
            Statements = [BuildStatement(operation)],
            Locals = [],
        };
    }
}