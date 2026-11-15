using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Parameters;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Types;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Statements;
using static RobloxCS.Lowering.Luau;
using TypeInfo = RobloxCS.AST.Types.TypeInfo;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private bool _returnsTuple;

    private static bool IsLuaTuple(ITypeSymbol? type) {
        return type is { Name: "LuaTuple" } && type.ContainingNamespace?.ToDisplayString() == "RobloxCS.Types";
    }

    private static bool ReturnsMultipleValues(IMethodSymbol method) {
        return IsLuaTuple(method.ReturnType) && (method.MethodKind == MethodKind.DelegateInvoke || GetRobloxNative(method.ContainingType) is not null);
    }

    private static TypeInfo TupleType() {
        return new TableTypeInfo {
            Fields = [
                new TypeField { Key = NameTypeFieldKey.FromString("n"), Value = Type("number") },
                new TypeField { Key = IndexSignatureTypeFieldKey.FromInfo(Type("number")), Value = Type("any") },
            ],
        };
    }

    private static TypeInfo TuplePackType() => Type("...any");

    private static FunctionCallExpression PackTuple(IEnumerable<Expression> values) => Call(Index("table", "pack"), values);

    private static FunctionCallExpression UnpackTuple(Expression tuple) {
        if (tuple is SymbolExpression) {
            return Call(Index("table", "unpack"), tuple, Num(1), Index(tuple, "n"));
        }

        return Call(Lambda([("tuple", TupleType())], TuplePackType(), [Return(UnpackTuple(Name("tuple")))]), tuple);
    }

    private List<Expression> LowerTupleValues(HirExpression tuple) {
        if (tuple is HirObjectCreation creation && IsLuaTuple(creation.Type)) {
            return LowerArguments(NativeArguments(creation.Arguments));
        }

        var value = LowerExpression(tuple);

        return [UnpackTuple(value is SymbolExpression ? value : CaptureValue(value, "tuple"))];
    }

    private ImmutableArray<HirArgument> SpreadTupleArgument(ImmutableArray<HirArgument> arguments, out bool spread) {
        spread = arguments is [.., { Value: HirObjectCreation creation } last] && IsLuaTuple(last.Symbol.Type) && IsLuaTuple(creation.Type);
        if (!spread) {
            return arguments;
        }

        var tuple = arguments[^1];
        var values = NativeArguments(((HirObjectCreation)tuple.Value).Arguments).Select(value => value with { Symbol = tuple.Symbol });

        return [..arguments[..^1], ..values];
    }

    private Expression LowerTupleCreation(HirObjectCreation creation) {
        return PackTuple(LowerArguments(NativeArguments(creation.Arguments)));
    }

    private Expression LowerTupleCall(HirCall call) {
        if (call is not { Method.Name: "Get", Receiver: { } receiver, Arguments: [var index] }) {
            Unsupported($"'{call.Method.ToDisplayString()}' is not supported on LuaTuple.", call.Location);

            return Nil();
        }

        var tuple = LowerExpression(receiver);
        var position = LowerExpression(index.Value);

        return TupleElement(tuple, position is NumberExpression number ? Num(number.Value + 1) : Binary(position, BinOp.Plus, Num(1)), call.Type, call.Location);
    }

    private Expression LowerTupleProperty(HirPropertyAccess access, HirExpression receiver) {
        if (access.Symbol.Name != "Count") {
            Unsupported($"'{access.Symbol.Name}' is not supported on LuaTuple.", access.Location);

            return Nil();
        }

        return Index(LowerExpression(receiver), "n");
    }

    private Expression TupleElement(Expression tuple, Expression position, ITypeSymbol? type, Location location) {
        Expression element = Index(tuple, position);

        return type is null || type.SpecialType == SpecialType.System_Object ? element : Cast(element, LowerType(type, location));
    }

    private void MakeVariadic(AnonymousFunctionExpression function, string name) {
        function.Body.Parameters[^1] = new EllipsisParameter();
        function.Body.TypeSpecifiers[^1] = Type("any");
        function.Body.Body.Statements.Insert(0, Local(name, TupleType(), PackTuple([Name("...")])));
    }

    private List<Statement> LowerDeconstruction(HirDeconstruction deconstruction) {
        if (!IsLuaTuple(deconstruction.Value.Type)) {
            Unsupported("Deconstruction is only supported on LuaTuple values yet.", deconstruction.Location);

            return [];
        }

        var tuple = CaptureValue(LowerExpression(deconstruction.Value), "tuple");
        var statements = new List<Statement>();

        for (var i = 0; i < deconstruction.Targets.Length; i++) {
            var target = deconstruction.Targets[i];
            var value = TupleElement(tuple, Num(i + 1), target.Type, target.Location);

            if (target.Declared is { } local) {
                statements.Add(Local(_names.NameFor(local), LowerType(local.Type, target.Location), value));
            } else if (target.Target is { } existing && LowerAssignmentTarget(existing) is { } assigned) {
                statements.Add(Assign(assigned, value));
            }
        }

        return statements;
    }
}
