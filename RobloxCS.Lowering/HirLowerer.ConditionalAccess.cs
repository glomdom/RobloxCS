using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Prefixes;
using RobloxCS.AST.Statements;
using RobloxCS.AST.Suffixes;
using RobloxCS.HIR.Expressions;
using RobloxCS.HIR.Statements;
using Serilog;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private readonly Stack<Expression> _conditionalReceivers = new();

    private Expression LowerConditionalAccess(HirConditionalAccess access) {
        var receiver = HoistUnlessCheap(LowerExpression(access.Receiver));

        _conditionalReceivers.Push(receiver);

        Expression value;
        List<Statement> preludes;

        try {
            (value, preludes) = LowerCaptured(access.WhenNotNull);
        } finally {
            _conditionalReceivers.Pop();
        }

        if (preludes.Count == 0) {
            return IfExpression.From(NotNil(receiver), value, Nil());
        }

        Log.Verbose("Conditional access needs temps, lowering to an if statement");

        var temp = _names.FreshTemp("access");

        _names.Emit(LocalAssignmentStatement.Naked(temp, Optional(LowerType(access.Type, access.Location))));
        _names.Emit(new IfStatement {
            Condition = NotNil(receiver),
            Block = BlockOf([.. preludes, Assign(VarName.FromString(temp), value)]),
        });

        return Name(temp);
    }

    private List<Statement> LowerConditionalStatement(HirConditionalStatement statement) {
        var receiver = HoistUnlessCheap(LowerExpression(statement.Receiver));

        _conditionalReceivers.Push(receiver);

        List<Statement> body;

        try {
            body = LowerStatement(statement.Body);
        } finally {
            _conditionalReceivers.Pop();
        }

        return [
            new IfStatement {
                Condition = NotNil(receiver),
                Block = BlockOf(body),
            },
        ];
    }

    private Expression LowerConditionalReceiver(HirConditionalReceiver receiver) {
        if (_conditionalReceivers.Count == 0) {
            Unsupported("Conditional access receiver used outside of a '?.' expression.", receiver.Location);

            return Nil();
        }

        return (Expression)_conditionalReceivers.Peek().DeepClone();
    }

    private Expression HoistUnlessCheap(Expression value) {
        if (value is VarExpression { Prefix: NamePrefix } var && var.Suffixes.All(suffix => suffix is Dot)) {
            return value;
        }

        return Hoist(value, "value");
    }

    private static BinaryOperatorExpression NotNil(Expression value) {
        return new BinaryOperatorExpression {
            Left = (Expression)value.DeepClone(),
            Op = BinOp.TildeEqual,
            Right = Nil(),
        };
    }
}