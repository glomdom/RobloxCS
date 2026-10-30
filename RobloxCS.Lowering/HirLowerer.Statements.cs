using RobloxCS.AST;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Statements;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Statements;
using Serilog;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private readonly Stack<List<HirStatement>> _continueIncrementors = new();
    private bool _returnsSelf;
    
    private List<Statement> LowerBlock(HirBlock? block) {
        if (block is null) {
            return [];
        }

        var statements = new List<Statement>();
        foreach (var statement in block.Statements) {
            statements.AddRange(LowerStatement(statement));

            if (statement is not (HirReturn or HirBreak or HirContinue)) continue;
            if (statement != block.Statements[^1]) {
                Log.Verbose("Dropping unreachable statements after {Terminator}", statement.GetType().Name);
            }

            break;
        }

        return statements;
    }

    private List<Statement> LowerStatement(HirStatement statement) {
        Log.Verbose("Lowering statement {StatementKind}", statement.GetType().Name);

        switch (statement) {
            case HirLocalDeclaration declaration: {
                return LowerLocalDeclaration(declaration);
            }

            case HirAssignment assignment: {
                return LowerAssignment(assignment);
            }

            case HirExpressionStatement expressionStatement: {
                return LowerExpressionStatement(expressionStatement);
            }

            case HirBlock block: {
                return [new DoStatement { Block = new Block { Statements = LowerBlock(block) } }];
            }

            case HirIf compare: {
                return LowerIf(compare);
            }

            case HirFor loop: {
                return LowerFor(loop);
            }

            case HirWhile loop: {
                return LowerWhile(loop);
            }

            case HirContinue stmt: {
                return LowerContinue(stmt);
            }
            
            case HirBreak: {
                return [new BreakStatement()];
            }

            case HirReturn returnStatement: {
                return LowerReturn(returnStatement);
            }

            default: {
                Unsupported($"Lowering '{statement.GetType().Name}' statements are not supported yet.", statement.Location);

                return [];
            }
        }
    }

    private List<Statement> LowerReturn(HirReturn statement) {
        if (_returnsSelf) {
            return [Return(Name("self"))];
        }

        if (statement.Value is null) {
            return [Return()];
        }

        return [Return(LowerExpression(statement.Value))];
    }

    private List<Statement> LowerContinue(HirContinue statement) {
        var statements = new List<Statement>();

        if (_continueIncrementors.TryPeek(out var incrementors) && incrementors.Count > 0) {
            Log.Verbose("Running {IncrementorCount} incrementors before continue", incrementors.Count);

            statements.AddRange(incrementors.SelectMany(LowerStatement));
        }

        statements.Add(new ContinueStatement());

        return statements;
    }

    private List<Statement> LowerIf(HirIf compare) {
        var elseIfs = new List<ElseIfBlock>();
        Block? elseBlock = null;

        var next = compare.Else;
        while (next is not null) {
            if (next is HirIf elseIf) {
                elseIfs.Add(new ElseIfBlock {
                    Condition = LowerExpression(elseIf.Condition),
                    Block = new Block { Statements = LowerBlock(elseIf.Then) },
                });

                next = elseIf.Else;

                continue;
            }

            elseBlock = new Block {
                Statements = next is HirBlock block ? LowerBlock(block) : LowerStatement(next),
            };

            break;
        }

        Log.Verbose("Lowering if with {ElseIfCount} elseif branches and {ElseKind}", elseIfs.Count, elseBlock is null ? "no else" : "an else");

        return [
            new IfStatement {
                Condition = LowerExpression(compare.Condition),
                Block = new Block { Statements = LowerBlock(compare.Then) },
                ElseIf = elseIfs.Count > 0 ? elseIfs : null,
                Else = elseBlock,
            },
        ];
    }
    
    private List<Statement> LowerFor(HirFor loop) {
        Log.Verbose(
            "Lowering for loop ({InitializerCount} initializers, {IncrementorCount} incrementors) to a while loop",
            loop.Initializers.Length, loop.Incrementors.Length
        );

        var initializers = loop.Initializers.SelectMany(LowerStatement).ToList();
        var condition = loop.Condition is { } hirCondition ? LowerExpression(hirCondition) : Bool(true);

        _continueIncrementors.Push([.. loop.Incrementors]);
        var body = LowerBlock(loop.Body);
        _continueIncrementors.Pop();

        if (body.LastOrDefault() is not (BreakStatement or ContinueStatement or ReturnStatement)) {
            body.AddRange(loop.Incrementors.SelectMany(LowerStatement));
        }

        var whileLoop = new WhileStatement {
            Condition = condition,
            Block = new Block { Statements = body },
        };

        if (initializers.Count == 0) {
            return [whileLoop];
        }

        return [
            new DoStatement {
                Block = new Block { Statements = [.. initializers, whileLoop] },
            },
        ];
    }
    
    private List<Statement> LowerWhile(HirWhile loop) {
        Log.Verbose("Lowering while loop");

        var condition = LowerExpression(loop.Condition);

        _continueIncrementors.Push([]);
        var body = LowerBlock(loop.Body);
        _continueIncrementors.Pop();

        return [
            new WhileStatement {
                Condition = condition,
                Block = new Block { Statements = body },
            },
        ];
    }

    private List<Statement> LowerLocalDeclaration(HirLocalDeclaration declaration) {
        var declarators = declaration.Declarators;
        var names = declarators.Select(declarator => _names.NameFor(declarator.Symbol)).ToList();
        var types = declarators.Select(declarator => LowerType(declarator.Type, declarator.Location)).ToList();

        for (var i = 0; i < declarators.Length; i++) {
            if (names[i] != declarators[i].Symbol.Name) {
                Log.Verbose("Renamed local {CSharpName} to {LuauName}", declarators[i].Symbol.Name, names[i]);
            }
        }

        if (declarators.All(declarator => declarator.Initializer is not null)) {
            Log.Verbose("Grouping {Locals} into a single local statement", names);

            return [
                new LocalAssignmentStatement {
                    Names = [.. names.Select(Name)],
                    Types = types,
                    Expressions = [.. declarators.Select(declarator => LowerExpression(declarator.Initializer!))],
                },
            ];
        }

        Log.Verbose("Splitting {Locals} into separate local statements since some have no initializer", names);

        var statements = new List<Statement>();
        for (var i = 0; i < declarators.Length; i++) {
            var initializer = declarators[i].Initializer is { } value ? LowerExpression(value) : null;

            statements.Add(Local(names[i], types[i], initializer));
        }

        return statements;
    }

    private List<Statement> LowerAssignment(HirAssignment assignment) {
        if (LowerAssignmentTarget(assignment.Target) is not { } target) {
            return [];
        }

        return [Assign(target, LowerExpression(assignment.Value))];
    }

    private Var? LowerAssignmentTarget(HirExpression target) {
        switch (target) {
            case HirLocalRef local: {
                return VarName.FromString(_names.NameFor(local.Symbol));
            }

            case HirParameterRef parameter: {
                return VarName.FromString(_names.NameFor(parameter.Symbol));
            }

            case HirPropertyAccess prop: {
                if (LowerPropertyAccess(prop) is Var var) {
                    return var;
                }

                Unsupported($"Property '{prop.Symbol.Name}' cannot be assigned to.", prop.Location);

                return null;
            }

            case HirFieldAccess field: {
                return LowerFieldAccess(field);
            }

            default: {
                Unsupported($"Assigning to '{target.GetType().Name}' is not supported yet.", target.Location);

                return null;
            }
        }
    }

    private List<Statement> LowerExpressionStatement(HirExpressionStatement statement) {
        if (statement.Expression is HirIncrementDecrement increment) {
            return LowerIncrementStatement(increment);
        }
        
        if (statement.Expression is not HirCall call) {
            Unsupported($"'{statement.Expression.GetType().Name}' can't be used as a statement yet.", statement.Location);

            return [];
        }

        if (LowerCall(call) is not { } lowered) {
            return [];
        }

        return [Stmt(lowered)];
    }
    
    private List<Statement> LowerIncrementStatement(HirIncrementDecrement increment) {
        if (LowerAssignmentTarget(increment.Target) is not { } target) {
            return [];
        }

        Log.Verbose("Lowering {Operator} to a compound assignment", increment.IsIncrement ? "++" : "--");

        return [
            new CompoundAssignmentStatement {
                Left = target,
                Operator = increment.IsIncrement ? CompoundOp.PlusEqual : CompoundOp.MinusEqual,
                Right = new VarExpression {
                    Prefix = new ExpressionPrefix {
                        Expression = Num(1),
                    },
                },
            },
        ];
    }
}