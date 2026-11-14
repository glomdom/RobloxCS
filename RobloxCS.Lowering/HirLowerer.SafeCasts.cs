using Microsoft.CodeAnalysis;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.HIR.Expressions;
using RobloxCS.Types;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private Expression LowerSafeCast(HirConversion conversion) {
        var target = conversion.Type.WithNullableAnnotation(NullableAnnotation.NotAnnotated);
        if (target.SpecialType == SpecialType.System_Object) {
            return conversion.Operand.Type.IsValueType ? LowerBoxing(conversion.Operand) : LowerExpression(conversion.Operand);
        }

        if (target is INamedTypeSymbol { OriginalDefinition.SpecialType: SpecialType.System_Nullable_T } nullable) {
            return LowerNullableSafeCast(conversion, nullable);
        }

        if (target.SpecialType is SpecialType.System_ValueType or SpecialType.System_Enum && conversion.Operand.Type.IsValueType) {
            return LowerBoxing(conversion.Operand);
        }

        var value = LowerExpression(conversion.Operand);
        Expression result;

        switch (target.SpecialType) {
            case SpecialType.System_ValueType or SpecialType.System_Enum: {
                result = LowerBoxReferenceCast(value, target.SpecialType == SpecialType.System_Enum);

                break;
            }

            case SpecialType.System_String: {
                result = CheckedValue(value, Equal(Call("typeof", Name("value")), Str("string")));

                break;
            }

            default: {
                if (GetRobloxNative(target) is { } native) {
                    var check = native.Kind is RobloxNativeType.Instance or RobloxNativeType.Service
                        ? new BinaryOperatorExpression {
                            Left = Equal(Call("typeof", Name("value")), Str("Instance")),
                            Op = BinOp.And,
                            Right = MethodCall(Name("value"), "IsA", Str(native.Name)),
                        }
                        : Equal(Call("typeof", Name("value")), Str(native.Name));

                    result = CheckedValue(value, check);
                } else if (target is INamedTypeSymbol named && IsTranspiledType(named) && named.TypeKind is TypeKind.Class or TypeKind.Interface) {
                    result = LowerReferenceSafeCast(value, named, conversion.Location);
                } else {
                    Unsupported($"Safe casts to '{target.ToDisplayString()}' are not supported yet.", conversion.Location);

                    return Nil();
                }

                break;
            }
        }

        return Cast(result, Optional(LowerType(target, conversion.Location)));
    }

    private Expression LowerNullableSafeCast(HirConversion conversion, INamedTypeSymbol target) {
        var underlying = target.TypeArguments[0];
        if (SymbolEqualityComparer.Default.Equals(conversion.Operand.Type, target) || SymbolEqualityComparer.Default.Equals(conversion.Operand.Type, underlying)) {
            return LowerExpression(conversion.Operand);
        }

        var value = LowerExpression(conversion.Operand);
        var result = LowerBoxCast(Name("value"), underlying, true);

        Expression? nativeCheck = null;
        if (GetRobloxNative(underlying) is { } native) {
            nativeCheck = Equal(Call("typeof", Name("value")), Str(native.Name));
        } else if (IsRobloxEnum(underlying)) {
            nativeCheck = new BinaryOperatorExpression {
                Left = Equal(Call("typeof", Name("value")), Str("EnumItem")),
                Op = BinOp.And,
                Right = Equal(Index("value", "EnumType"), Index("Enum", underlying.Name)),
            };
        }

        if (nativeCheck is null) {
            return Cast(LowerBoxCast(value, underlying, true), LowerType(target, conversion.Location));
        }

        result = IfExpression.From(nativeCheck, Name("value"), result);

        return Cast(Call(Lambda([("value", Type("any"))], Type("any"), [Return(result)]), value), LowerType(target, conversion.Location));
    }

    private Expression LowerReferenceSafeCast(Expression value, INamedTypeSymbol target, Location location) {
        var iteration = new List<Statement>();
        if (target.TypeKind == TypeKind.Class) {
            iteration.Add(new IfStatement {
                Condition = Equal(Name("class"), Name("target")),
                Block = BlockOf([Return(Name("value"))]),
            });
        }

        iteration.AddRange([
            Local("metadata", Type("any"), Call("getmetatable", Name("class"))),
            new IfStatement {
                Condition = NotEqual(Call("typeof", Name("metadata")), Str("table")),
                Block = BlockOf([Return(Nil())]),
            },
        ]);

        if (target.TypeKind == TypeKind.Interface) {
            iteration.AddRange([
                Local("interfaces", Type("any"), Call("rawget", Name("metadata"), Str("__interfaces"))),
                new IfStatement {
                    Condition = new BinaryOperatorExpression {
                        Left = Equal(Call("typeof", Name("interfaces")), Str("table")),
                        Op = BinOp.And,
                        Right = Equal(Call("rawget", Name("interfaces"), Name("target")), Bool(true)),
                    },
                    Block = BlockOf([Return(Name("value"))]),
                },
            ]);
        }

        iteration.Add(Assign(VarName.FromString("class"), Call("rawget", Name("metadata"), Str("__base"))));
        var body = new List<Statement> {
            new IfStatement {
                Condition = NotEqual(Call("typeof", Name("value")), Str("table")),
                Block = BlockOf([Return(Nil())]),
            },
            Local("class", Type("any"), Call("getmetatable", Name("value"))),
            new WhileStatement { Condition = Equal(Call("typeof", Name("class")), Str("table")), Block = BlockOf(iteration) },
            Return(Nil()),
        };

        return Call(
            Lambda([("value", Type("any")), ("target", Type("any"))], Type("any"), body), value,
            target.TypeKind == TypeKind.Interface ? Str(RuntimeTypeKey(target)) : Name(ReferenceType(target, location))
        );
    }

    private static Expression CheckedValue(Expression value, Expression condition) =>
        Call(Lambda([("value", Type("any"))], Type("any"), [Return(IfExpression.From(condition, Name("value"), Nil()))]), value);

    private static BinaryOperatorExpression Equal(Expression left, Expression right) => new() { Left = left, Op = BinOp.TwoEqual, Right = right };
    private static BinaryOperatorExpression NotEqual(Expression left, Expression right) => new() { Left = left, Op = BinOp.TildeEqual, Right = right };
}