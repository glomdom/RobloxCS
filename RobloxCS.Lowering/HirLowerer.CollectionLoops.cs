using RobloxCS.AST;
using RobloxCS.AST.Statements;
using RobloxCS.HIR.Statements;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private List<Statement> LowerForEach(HirForEach loop) {
        if (CollectionElement(loop.Collection.Type) is not { } element) {
            Unsupported("foreach currently requires an array or a supported generic collection.", loop.Location);

            return [];
        }

        var collection = CaptureValue(LowerExpression(loop.Collection), "enumerable");
        var index = _names.FreshTemp("index");
        var version = _names.FreshTemp("version");

        _continueIncrementors.Push(Increment);
        var body = LowerBlock(loop.Body);
        _continueIncrementors.Pop();

        var value = ConvertCollectionElement(CollectionCall("get", collection, Name(index)), element, loop.Variable.Type, loop.Location);
        List<Statement> iteration = [
            CollectionIf(
                Binary(
                    Equal(Index(collection, "array"), Bool(false)),
                    BinOp.And,
                    NotEqual(Index(collection, "version"), Name(version))
                ), Stmt(Call("error", Str("Collection was modified during enumeration.")))
            ),
            CollectionIf(Binary(Name(index), BinOp.GreaterThanEqual, Index(collection, "count")), new BreakStatement()),
            Local(_names.NameFor(loop.Variable), LowerType(loop.Variable.Type, loop.Location), Cast(value, LowerType(loop.Variable.Type, loop.Location))),
            ..body,
        ];

        if (body.LastOrDefault() is not (ReturnStatement or BreakStatement or ContinueStatement)) {
            iteration.AddRange(Increment());
        }

        return [
            new DoStatement {
                Block = BlockOf([
                    Local(index, Num(0)), Local(version, Index(collection, "version")),
                    CollectionWhile(Bool(true), iteration),
                ])
            }
        ];

        List<Statement> Increment() => [Assign(VarName.FromString(index), Binary(Name(index), BinOp.Plus, Num(1)))];
    }
}