using Microsoft.CodeAnalysis;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.HIR;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Patterns;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private readonly Stack<List<Statement>> _patternLocals = new();
    private readonly Stack<Expression> _patternInputs = new();

    private Expression LowerIsPattern(HirIsPattern isPattern) {
        var subject = LowerExpression(isPattern.Operand);
        if (!UsesSubjectOnce(isPattern.Pattern)) {
            subject = Hoist(subject, "subject");
        }

        return LowerPattern(subject, isPattern.Operand.Type, isPattern.Pattern);
    }

    private static bool UsesSubjectOnce(HirPattern pattern) {
        return pattern switch {
            HirNotPattern not => UsesSubjectOnce(not.Pattern),
            HirTypePattern or HirVarPattern or HirConstantPattern => true,

            _ => false,
        };
    }

    private Expression LowerPattern(Expression subject, ITypeSymbol subjectType, HirPattern pattern) {
        switch (pattern) {
            case HirVarPattern var: {
                if (var.Declared is { } local && DeclarePatternLocal(local, var.Location) is { } name) {
                    _names.Emit(Assign(VarName.FromString(name), subject));
                }

                return Bool(true);
            }

            case HirTypePattern type: {
                var value = LowerTypeTest(subject, subjectType, type.TestType, type.Location);
                if (type.Declared is not { } local || DeclarePatternLocal(local, type.Location) is not { } name) {
                    return NotEqual(value, Nil());
                }

                _names.Emit(Assign(VarName.FromString(name), Cast(value, Type("any"))));

                return NotEqual(Name(name), Nil());
            }

            case HirConstantPattern constant: {
                return LowerConstantPattern(subject, subjectType, constant);
            }

            case HirRelationalPattern relational: {
                return LowerRelationalPattern(subject, subjectType, relational);
            }

            case HirNotPattern not: {
                return new UnaryOperatorExpression { UnOp = UnOp.Not, Expression = LowerPattern(subject, subjectType, not.Pattern) };
            }

            case HirBinaryPattern binary: {
                var left = LowerPattern(subject, subjectType, binary.Left);
                var (right, preludes) = LowerCapturedPattern(subject, subjectType, binary.Right);

                return ShortCircuit(left, binary.IsAnd ? BinOp.And : BinOp.Or, right, preludes);
            }

            case HirRecursivePattern recursive: {
                return LowerRecursivePattern(subject, subjectType, recursive);
            }

            default: {
                Unsupported($"Patterns of kind '{pattern.GetType().Name}' are not supported yet.", pattern.Location);

                return Bool(false);
            }
        }
    }

    private (Expression Condition, List<Statement> Preludes) LowerCapturedPattern(Expression subject, ITypeSymbol subjectType, HirPattern pattern) {
        Expression condition = Nil();
        var preludes = _names.Capture(() => condition = LowerPattern(subject, subjectType, pattern));

        return (condition, preludes);
    }

    private Expression LowerRecursivePattern(Expression subject, ITypeSymbol subjectType, HirRecursivePattern pattern) {
        var matched = pattern.MatchedType ?? subjectType;
        var name = pattern.Declared is { } local ? DeclarePatternLocal(local, pattern.Location) : null;

        Expression input;
        Expression condition;

        if (SymbolEqualityComparer.Default.Equals(Underlying(subjectType), Underlying(matched))) {
            if (name is not null) {
                _names.Emit(Assign(VarName.FromString(name), subject));
            }

            input = name is not null ? Name(name) : subject;
            condition = SymbolEqualityComparer.Default.Equals(Underlying(subjectType), subjectType) && subjectType.IsValueType
                ? Bool(true)
                : NotEqual(input, Nil());
        } else {
            var value = LowerTypeTest(subject, subjectType, matched, pattern.Location);
            if (name is not null) {
                _names.Emit(Assign(VarName.FromString(name), Cast(value, Type("any"))));
            }

            input = name is not null ? Name(name) : Hoist(Cast(value, Type("any")), "input");
            condition = NotEqual(input, Nil());
        }

        foreach (var property in pattern.Properties) {
            Expression part = Nil();

            _patternInputs.Push(input);

            var preludes = _names.Capture(() => {
                var member = LowerExpression(property.Member);
                if (property.Pattern is HirRecursivePattern) {
                    member = Hoist(Cast(member, Type("any")), "member");
                } else if (!UsesSubjectOnce(property.Pattern)) {
                    member = Hoist(member, "member");
                }

                part = LowerPattern(member, property.Member.Type, property.Pattern);
            });

            _patternInputs.Pop();

            if (condition is BooleanExpression { Value: true }) {
                foreach (var prelude in preludes) {
                    _names.Emit(prelude);
                }

                condition = part;
            } else {
                condition = ShortCircuit(condition, BinOp.And, part, preludes);
            }
        }

        return condition;
    }

    private string? DeclarePatternLocal(ILocalSymbol local, Location location) {
        if (!_patternLocals.TryPeek(out var locals)) {
            Unsupported("Pattern variables are only supported inside statements.", location);

            return null;
        }

        var name = _names.NameFor(local);
        locals.Add(LocalAssignmentStatement.Naked(name, LowerType(local.Type, location)));

        return name;
    }

    private Expression LowerTypeTest(Expression subject, ITypeSymbol subjectType, ITypeSymbol testType, Location location) {
        var target = Underlying(testType.WithNullableAnnotation(NullableAnnotation.NotAnnotated));

        if (subjectType.IsValueType) {
            if (SymbolEqualityComparer.Default.Equals(Underlying(subjectType), target)) {
                return subject;
            }

            Unsupported($"Type tests from '{subjectType.ToDisplayString()}' to '{target.ToDisplayString()}' are not supported yet.", location);

            return Nil();
        }

        if (target.SpecialType == SpecialType.System_Object) {
            return subject;
        }

        return target.IsValueType ? LowerBoxCast(subject, target, true) : LowerSafeCastValue(subject, target, location);
    }

    private Expression LowerConstantPattern(Expression subject, ITypeSymbol subjectType, HirConstantPattern pattern) {
        var constant = UnwrapPatternValue(pattern.Value);
        if (constant is HirLiteral { Value: null }) {
            return Equal(subject, Nil());
        }

        var value = LowerExpression(constant);

        return subjectType.IsReferenceType && constant.Type.IsValueType
            ? Equal(LowerBoxCast(subject, constant.Type, true), value)
            : Equal(subject, value);
    }

    private Expression LowerRelationalPattern(Expression subject, ITypeSymbol subjectType, HirRelationalPattern pattern) {
        if (subjectType.IsReferenceType || ToBinOp(pattern.Op) is not { } op) {
            Unsupported($"Relational patterns on '{subjectType.ToDisplayString()}' are not supported yet.", pattern.Location);

            return Bool(false);
        }

        var comparison = Binary(subject, op, LowerExpression(UnwrapPatternValue(pattern.Value)));
        if (!SymbolEqualityComparer.Default.Equals(Underlying(subjectType), subjectType)) {
            return Binary(NotEqual(subject, Nil()), BinOp.And, comparison);
        }

        return comparison;
    }

    private static HirExpression UnwrapPatternValue(HirExpression value) {
        while (
            value is HirConversion {
                Kind: HirConversionKind.Boxing or HirConversionKind.Reference or HirConversionKind.Identity or HirConversionKind.NullableWrap
            } conversion
        ) {
            value = conversion.Operand;
        }

        return value;
    }

    private static ITypeSymbol Underlying(ITypeSymbol type) {
        return type is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } nullable ? nullable.TypeArguments[0] : type;
    }
}