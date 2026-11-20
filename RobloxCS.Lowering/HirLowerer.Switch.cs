using Microsoft.CodeAnalysis;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Patterns;
using RobloxCS.HIR.Statements;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private Expression LowerSwitchExpression(HirSwitchExpression switchExpression) {
        var subject = Hoist(LowerExpression(switchExpression.Value), "subject");
        var subjectType = switchExpression.Value.Type;

        var arms = new List<(Expression Condition, List<Statement> Preludes, Expression Value, List<Statement> ValuePreludes)>();
        var exhaustive = false;

        foreach (var arm in switchExpression.Arms) {
            var (condition, preludes) = LowerArmCondition(subject, subjectType, arm.Pattern, arm.Guard);
            var (value, valuePreludes) = LowerCaptured(arm.Value);

            arms.Add((condition, preludes, value, valuePreludes));

            if (arm is { Pattern: HirVarPattern, Guard: null }) {
                exhaustive = true;

                break;
            }
        }

        var simple = exhaustive && arms.All(arm => arm.Preludes.Count == 0 && arm.ValuePreludes.Count == 0);
        if (simple) {
            if (arms.Count == 1) {
                return arms[0].Value;
            }

            return new IfExpression {
                Condition = arms[0].Condition,
                If = arms[0].Value,
                ElseIfExpressions = arms.Count > 2 ? [..arms.Skip(1).SkipLast(1).Select(arm => ElseIfExpression.From(arm.Condition, arm.Value))] : null,
                Else = arms[^1].Value,
            };
        }

        var result = _names.FreshTemp("result");
        _names.Emit(LocalAssignmentStatement.Naked(result, LowerType(switchExpression.Type, switchExpression.Location)));

        var branches = arms.Select(arm => (arm.Condition, arm.Preludes, Body: (List<Statement>)[..arm.ValuePreludes, Assign(VarName.FromString(result), arm.Value)])).ToList();
        List<Statement> otherwise = [Stmt(Call("error", Str("The switch expression did not match its value.")))];

        if (exhaustive) {
            var last = branches[^1];
            branches.RemoveAt(branches.Count - 1);
            otherwise = [..last.Preludes, ..last.Body];
        }

        foreach (var statement in IfChain(branches, otherwise)) {
            _names.Emit(statement);
        }

        return Name(result);
    }

    private List<Statement> LowerSwitch(HirSwitch switchStatement) {
        var subject = Hoist(LowerExpression(switchStatement.Value), "subject");
        var subjectType = switchStatement.Value.Type;

        var branches = new List<(Expression Condition, List<Statement> Preludes, List<Statement> Body)>();
        List<Statement>? otherwise = null;

        foreach (var section in switchStatement.Sections) {
            if (section.Labels.Any(label => label.Pattern is null)) {
                otherwise = LowerBlock(section.Body);

                continue;
            }

            Expression? condition = null;
            var preludes = _names.Capture(() => {
                foreach (var label in section.Labels) {
                    var (labelCondition, labelPreludes) = LowerArmCondition(subject, subjectType, label.Pattern!, label.Guard);
                    if (condition is not null) {
                        condition = ShortCircuit(condition, BinOp.Or, labelCondition, labelPreludes);

                        continue;
                    }

                    foreach (var prelude in labelPreludes) {
                        _names.Emit(prelude);
                    }

                    condition = labelCondition;
                }
            });

            branches.Add((condition ?? Bool(false), preludes, LowerBlock(section.Body)));
        }

        return IfChain(branches, otherwise);
    }

    private (Expression Condition, List<Statement> Preludes) LowerArmCondition(Expression subject, ITypeSymbol subjectType, HirPattern pattern, HirExpression? guard) {
        Expression condition = Nil();
        var preludes = _names.Capture(() => {
            condition = LowerPattern(subject, subjectType, pattern);
            if (guard is null) {
                return;
            }

            var (guardValue, guardPreludes) = LowerCaptured(guard);
            condition = condition is BooleanExpression { Value: true } && guardPreludes.Count == 0
                ? guardValue
                : ShortCircuit(condition, BinOp.And, guardValue, guardPreludes);
        });

        return (condition, preludes);
    }

    private static List<Statement> IfChain(List<(Expression Condition, List<Statement> Preludes, List<Statement> Body)> branches, List<Statement>? otherwise) {
        if (branches.Count == 0) {
            return otherwise ?? [];
        }

        var elseIfs = new List<ElseIfBlock>();
        Block? elseBlock = null;

        for (var i = 1; i < branches.Count; i++) {
            if (branches[i].Preludes.Count > 0) {
                elseBlock = BlockOf(IfChain([..branches.Skip(i)], otherwise));
                otherwise = null;

                break;
            }

            elseIfs.Add(new ElseIfBlock { Condition = branches[i].Condition, Block = BlockOf(branches[i].Body) });
        }

        if (otherwise is { Count: > 0 }) {
            elseBlock = BlockOf(otherwise);
        }

        return [
            ..branches[0].Preludes,
            new IfStatement {
                Condition = branches[0].Condition,
                Block = BlockOf(branches[0].Body),
                ElseIf = elseIfs.Count > 0 ? elseIfs : null,
                Else = elseBlock,
            },
        ];
    }
}