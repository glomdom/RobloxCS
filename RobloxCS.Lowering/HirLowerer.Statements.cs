using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Statements;
using Serilog;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private List<Statement> LowerBlock(HirBlock? block) {
        if (block is null) {
            Log.Verbose("Block is null, lowering to an empty body");

            return [];
        }

        return [.. block.Statements.SelectMany(LowerStatement)];
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

            default: {
                Unsupported($"'{statement.GetType().Name}' statements are not supported yet.", statement.Location);

                return [];
            }
        }
    }

    private List<Statement> LowerLocalDeclaration(HirLocalDeclaration declaration) {
        var declarators = declaration.Declarators;
        var names = declarators.Select(declarator => _names.NameFor(declarator.Symbol)).ToList();
        var types = declarators.Select(declarator => LowerType(declarator.Symbol.Type, declarator.Location)).ToList();

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
        if (statement.Expression is not HirCall call) {
            Unsupported($"'{statement.Expression.GetType().Name}' can't be used as a statement yet.", statement.Location);

            return [];
        }

        if (LowerCall(call) is not FunctionCallExpression lowered) {
            return [];
        }

        return [Stmt(lowered)];
    }
}