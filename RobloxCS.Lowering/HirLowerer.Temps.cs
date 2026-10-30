using Microsoft.CodeAnalysis.Operations;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Statements;
using Serilog;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private List<Statement> LowerStatement(HirStatement statement) {
        List<Statement> lowered = [];
        var preludes = _names.Capture(() => lowered = LowerStatementCore(statement));

        if (preludes.Count == 0) {
            return lowered;
        }

        Log.Verbose("Emitting {Count} prelude statements before {StatementKind}", preludes.Count, statement.GetType().Name);

        return [.. preludes, .. lowered];
    }

    private Expression Hoist(Expression value, string hint = "t") {
        if (value is SymbolExpression or NumberExpression or StringExpression or BooleanExpression) {
            return value;
        }

        var name = _names.FreshTemp(hint);
        _names.Emit(Local(name, value));

        return Name(name);
    }

    private (Expression Value, List<Statement> Preludes) LowerCaptured(HirExpression expression) {
        Expression value = Nil();
        var preludes = _names.Capture(() => value = LowerExpression(expression));

        return (value, preludes);
    }

    private Expression LowerShortCircuit(HirBinary binary) {
        var left = LowerExpression(binary.Left);
        var (right, preludes) = LowerCaptured(binary.Right);
        var op = binary.Op is BinaryOperatorKind.ConditionalAnd ? BinOp.And : BinOp.Or;

        if (preludes.Count == 0) {
            return new BinaryOperatorExpression {
                Left = left,
                Op = op,
                Right = right,
            };
        }

        Log.Verbose("Right side of {Operator} needs temps, lowering to an if statement", op);

        var temp = _names.FreshTemp("cond");
        _names.Emit(Local(temp, left));

        Expression check = op is BinOp.And
            ? Name(temp)
            : new UnaryOperatorExpression { UnOp = UnOp.Not, Expression = Name(temp) };

        _names.Emit(new IfStatement {
            Condition = check,
            Block = BlockOf([.. preludes, Assign(VarName.FromString(temp), right)]),
        });

        return Name(temp);
    }

    private Expression LowerConditional(HirConditional conditional) {
        var condition = LowerExpression(conditional.Condition);
        var (whenTrue, truePreludes) = LowerCaptured(conditional.WhenTrue);
        var (whenFalse, falsePreludes) = LowerCaptured(conditional.WhenFalse);

        if (truePreludes.Count == 0 && falsePreludes.Count == 0) {
            if (whenFalse is IfExpression nested) {
                return new IfExpression {
                    Condition = condition,
                    If = whenTrue,
                    ElseIfExpressions = [ElseIfExpression.From(nested.Condition, nested.If), .. nested.ElseIfExpressions ?? []],
                    Else = nested.Else,
                };
            }

            return IfExpression.From(condition, whenTrue, whenFalse);
        }

        Log.Verbose("Conditional branch needs temps, lowering to an if statement");

        var temp = _names.FreshTemp("cond");
        _names.Emit(LocalAssignmentStatement.Naked(temp, LowerType(conditional.Type, conditional.Location)));
        _names.Emit(new IfStatement {
            Condition = condition,
            Block = BlockOf([.. truePreludes, Assign(VarName.FromString(temp), whenTrue)]),
            Else = BlockOf([.. falsePreludes, Assign(VarName.FromString(temp), whenFalse)]),
        });

        return Name(temp);
    }

    private (Expression Condition, List<Statement> BodyPrefix) LowerLoopCondition(HirExpression condition) {
        var (value, preludes) = LowerCaptured(condition);

        if (preludes.Count == 0) {
            return (value, []);
        }

        Log.Verbose("Loop condition needs temps, moving it into the loop body");

        var exit = new IfStatement {
            Condition = new UnaryOperatorExpression { UnOp = UnOp.Not, Expression = value },
            Block = BlockOf([new BreakStatement()]),
        };

        return (Bool(true), [.. preludes, exit]);
    }

    private static Block BlockOf(List<Statement> statements) => new() { Statements = statements };
}