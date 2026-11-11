using System.Globalization;
using Microsoft.CodeAnalysis;
using RobloxCS.AST;
using RobloxCS.AST.Expressions;
using RobloxCS.AST.Statements;
using RobloxCS.HIR;
using RobloxCS.HIR.Declarations;
using RobloxCS.HIR.Expressions;
using static RobloxCS.Lowering.Luau;

namespace RobloxCS.Lowering;

public sealed partial class HirLowerer {
    private static string EnumFormatterName(INamedTypeSymbol type) {
        var name = "__format";
        while (type.GetMembers(name).Length > 0) {
            name += "_";
        }

        return name;
    }

    private static bool IsFlagsEnum(INamedTypeSymbol type) => type.GetAttributes().Any(attribute => attribute.AttributeClass?.ToDisplayString() == "System.FlagsAttribute");

    private FunctionDeclarationStatement LowerEnumFormatter(HirType type) {
        var body = new List<Statement>();
        var members = type.Fields.GroupBy(field => field.Symbol.ConstantValue).Select(group => group.First()).ToList();

        foreach (var field in members) {
            body.Add(new IfStatement {
                Condition = new BinaryOperatorExpression {
                    Left = Name("value"), Op = BinOp.TwoEqual, Right = Num(Convert.ToDouble(field.Symbol.ConstantValue, CultureInfo.InvariantCulture))
                },
                Block = BlockOf([Return(Str(field.Name))]),
            });
        }

        if (IsFlagsEnum(type.Symbol) && type.Symbol.EnumUnderlyingType!.SpecialType is not (SpecialType.System_Int64 or SpecialType.System_UInt64)) {
            body.Add(Local("remaining", Call(Index("bit32", "bor"), Name("value"))));
            body.Add(Local("names", Table()));

            var sorted = members.Select(field => (Field: field, Mask: unchecked((uint)Convert.ToInt64(field.Symbol.ConstantValue, CultureInfo.InvariantCulture))))
                .Where(entry => entry.Mask != 0).OrderByDescending(entry => entry.Mask);

            foreach (var (field, mask) in sorted) {
                body.Add(new IfStatement {
                    Condition = new BinaryOperatorExpression { Left = Call(Index("bit32", "band"), Name("remaining"), Num(mask)), Op = BinOp.TwoEqual, Right = Num(mask) },
                    Block = BlockOf([
                        Assign(VarName.FromString("remaining"), Call(Index("bit32", "bxor"), Name("remaining"), Num(mask))),
                        Stmt(Call(Index("table", "insert"), Name("names"), Num(1), Str(field.Name))),
                    ]),
                });
            }

            body.Add(new IfStatement {
                Condition = new BinaryOperatorExpression {
                    Left = new BinaryOperatorExpression { Left = Name("remaining"), Op = BinOp.TwoEqual, Right = Num(0) },
                    Op = BinOp.And,
                    Right = new BinaryOperatorExpression { Left = Name("value"), Op = BinOp.TildeEqual, Right = Num(0) },
                },
                Block = BlockOf([Return(Call(Index("table", "concat"), Name("names"), Str(", ")))]),
            });
        }

        body.Add(Return(Call("tostring", Name("value"))));
        return Function(TypeName(type.Symbol), EnumFormatterName(type.Symbol), [("value", Type("number"))], Type("string"), body);
    }

    private static HirExpression UnwrapBox(HirExpression expression) {
        while (expression is HirConversion { Kind: HirConversionKind.Boxing } conversion) {
            expression = conversion.Operand;
        }

        return expression;
    }
}