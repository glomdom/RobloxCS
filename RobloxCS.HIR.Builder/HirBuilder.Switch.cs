using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Patterns;
using RobloxCS.HIR.Statements;

namespace RobloxCS.HIR.Builder;

public sealed partial class HirBuilder {
    private readonly HashSet<ILabelSymbol> _switchExits = new(SymbolEqualityComparer.Default);

    private HirSwitchExpression BuildSwitchExpression(ISwitchExpressionOperation operation) {
        return new HirSwitchExpression {
            Location = operation.Syntax.GetLocation(),
            Type = operation.Type!,
            Value = BuildExpression(operation.Value),
            Arms = [
                ..operation.Arms.Select(arm => new HirSwitchArm {
                    Location = arm.Syntax.GetLocation(),
                    Pattern = BuildPattern(arm.Pattern),
                    Guard = arm.Guard is { } guard ? BuildExpression(guard) : null,
                    Value = BuildExpression(arm.Value),
                }),
            ],
        };
    }

    private HirSwitch BuildSwitch(ISwitchOperation operation) {
        _switchExits.Add(operation.ExitLabel);

        try {
            return new HirSwitch {
                Location = operation.Syntax.GetLocation(),
                Value = BuildExpression(operation.Value),
                Sections = [..operation.Cases.Select(section => BuildSwitchSection(section, operation.ExitLabel))],
            };
        } finally {
            _switchExits.Remove(operation.ExitLabel);
        }
    }

    private HirSwitchSection BuildSwitchSection(ISwitchCaseOperation section, ILabelSymbol exit) {
        var location = section.Syntax.GetLocation();

        return new HirSwitchSection {
            Location = location,
            Labels = [..section.Clauses.Select(BuildSwitchLabel)],
            Body = new HirBlock {
                Location = location,
                Statements = [..BuildSectionBody(section.Body, exit)],
                Locals = [..section.Locals],
            },
        };
    }

    private List<HirStatement> BuildSectionBody(IEnumerable<IOperation> operations, ILabelSymbol exit) {
        var body = operations.ToList();
        if (body is [.., IBranchOperation { BranchKind: BranchKind.Break } last] && SymbolEqualityComparer.Default.Equals(last.Target, exit)) {
            return [..body[..^1].Select(BuildStatement)];
        }

        if (body is [.., IBlockOperation block]) {
            return [
                ..body[..^1].Select(BuildStatement),
                new HirBlock {
                    Location = block.Syntax.GetLocation(),
                    Statements = [..BuildSectionBody(block.Operations, exit)],
                    Locals = [..block.Locals],
                },
            ];
        }

        return [..body.Select(BuildStatement)];
    }

    private HirSwitchLabel BuildSwitchLabel(ICaseClauseOperation clause) {
        var location = clause.Syntax.GetLocation();

        return clause switch {
            IDefaultCaseClauseOperation => new HirSwitchLabel { Location = location },
            ISingleValueCaseClauseOperation single => new HirSwitchLabel {
                Location = location,
                Pattern = new HirConstantPattern { Location = location, Value = BuildExpression(single.Value) },
            },

            IPatternCaseClauseOperation pattern => new HirSwitchLabel {
                Location = location,
                Pattern = BuildPattern(pattern.Pattern),
                Guard = pattern.Guard is { } guard ? BuildExpression(guard) : null,
            },

            _ => throw new NotSupportedException($"Case clauses of kind '{clause.CaseKind}' are not supported."),
        };
    }

    private HirRecursivePattern BuildRecursivePattern(IRecursivePatternOperation operation) {
        if (!operation.DeconstructionSubpatterns.IsEmpty) {
            throw new NotSupportedException("Positional patterns are not supported yet.");
        }

        return new HirRecursivePattern {
            Location = operation.Syntax.GetLocation(),
            MatchedType = operation.MatchedType,
            Declared = operation.DeclaredSymbol as ILocalSymbol,
            Properties = [..operation.PropertySubpatterns.Select(BuildPropertySubpattern)],
        };
    }

    private HirPropertySubpattern BuildPropertySubpattern(IPropertySubpatternOperation operation) {
        var location = operation.Syntax.GetLocation();
        var chain = new List<IMemberReferenceOperation>();

        var current = operation.Member;
        while (current is IMemberReferenceOperation member) {
            chain.Insert(0, member);
            current = member.Instance;
        }

        if (chain.Count == 0 || current is not IInstanceReferenceOperation { ReferenceKind: InstanceReferenceKind.PatternInput }) {
            throw new NotSupportedException("This property pattern member is not supported yet.");
        }

        var pattern = BuildPattern(operation.Pattern);
        for (var i = chain.Count - 1; i > 0; i--) {
            pattern = new HirRecursivePattern {
                Location = location,
                MatchedType = chain[i - 1].Type,
                Properties = [new HirPropertySubpattern { Location = location, Member = MemberOfInput(chain[i], chain[i - 1].Type!), Pattern = pattern }],
            };
        }

        return new HirPropertySubpattern { Location = location, Member = BuildExpression(chain[0]), Pattern = pattern };
    }

    private HirExpression MemberOfInput(IMemberReferenceOperation member, ITypeSymbol inputType) {
        var input = new HirPatternInput { Location = member.Syntax.GetLocation(), Type = inputType };

        return BuildExpression(member) switch {
            HirPropertyAccess property => property with { Receiver = input },
            HirFieldAccess field => field with { Receiver = input },

            _ => throw new NotSupportedException("This property pattern member is not supported yet."),
        };
    }
}