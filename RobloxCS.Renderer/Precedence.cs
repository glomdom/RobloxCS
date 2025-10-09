using RobloxCS.AST;

namespace RobloxCS.Renderer;

public static class Precedence {
    private static readonly Dictionary<BinOp, int> LuauPrecedence = new() {
        { BinOp.Caret, 8 },
        { BinOp.Star, 6 },
        { BinOp.Slash, 6 },
        { BinOp.Percent, 6 },
        { BinOp.Plus, 5 },
        { BinOp.Minus, 5 },
        { BinOp.TwoDots, 4 },
        { BinOp.LessThan, 3 },
        { BinOp.LessThanEqual, 3 },
        { BinOp.GreaterThan, 3 },
        { BinOp.GreaterThanEqual, 3 },
        { BinOp.TwoEqual, 3 },
        { BinOp.TildeEqual, 3 },
        { BinOp.And, 2 },
        { BinOp.Or, 1 },
    };

    public static bool IsRightAssociative(BinOp op) => op is BinOp.Caret or BinOp.TwoDots;
    public static int Get(BinOp op) => LuauPrecedence.GetValueOrDefault(op, int.MaxValue);
}