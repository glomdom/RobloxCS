using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private static Expression LowerRuntimeDisplay(Expression value) {
        var metadataBody = new List<Statement> {
            new IfStatement {
                Condition = NotEqual(Call("rawget", Name("metadata"), Str("__boxedType")), Nil()),
                Block = BlockOf([
                    Return(Cast(Call(Cast(Call("rawget", Name("metadata"), Str("__format")), Type("any")), Call("rawget", Name("value"), Str("__value"))), Type("string")))
                ]),
            },
            Local("format", Type("any"), Index(Cast(Name("value"), Type("any")), "ToString")),
            new IfStatement {
                Condition = Equal(Call("typeof", Name("format")), Str("function")),
                Block = BlockOf([Return(Cast(Call(Cast(Name("format"), Type("any")), Name("value")), Type("string")))]),
            },
            new IfStatement {
                Condition = NotEqual(Call("rawget", Name("metadata"), Str("__name")), Nil()),
                Block = BlockOf([Return(Cast(Call("rawget", Name("metadata"), Str("__name")), Type("string")))]),
            },
        };

        var body = new List<Statement> {
            new IfStatement { Condition = Equal(Name("value"), Nil()), Block = BlockOf([Return(Str(""))]) },
            new IfStatement {
                Condition = Equal(Call("typeof", Name("value")), Str("boolean")),
                Block = BlockOf([Return(IfExpression.From(Name("value"), Str("True"), Str("False")))]),
            },
            new IfStatement {
                Condition = Equal(Call("typeof", Name("value")), Str("table")),
                Block = BlockOf([
                    Local("class", Type("any"), Call("getmetatable", Name("value"))),
                    new IfStatement {
                        Condition = Equal(Call("typeof", Name("class")), Str("table")),
                        Block = BlockOf([
                            Local("metadata", Type("any"), Call("getmetatable", Name("class"))),
                            new IfStatement { Condition = Equal(Call("typeof", Name("metadata")), Str("table")), Block = BlockOf(metadataBody) },
                        ]),
                    },
                ]),
            },
            Return(Call("tostring", Name("value"))),
        };

        return Call(Lambda([("value", Type("any"))], Type("string"), body), value);
    }
}