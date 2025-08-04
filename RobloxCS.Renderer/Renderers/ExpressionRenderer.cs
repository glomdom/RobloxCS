using RobloxCS.AST;
using RobloxCS.AST.Expressions;

namespace RobloxCS.Renderer.Renderers;

public class ExpressionRenderer : IRenderer<Expression> {
    public void Render(RenderState state, Expression node) {
        if (node is BooleanExpression boolean) {
            RenderBoolean(state, boolean);
        } else if (node is SymbolExpression sym) {
            RenderSymbol(state, sym);
        }
    }

    private static void RenderBoolean(RenderState state, BooleanExpression expr) {
        state.Builder.Append(expr.Value ? "true" : "false");
    }

    private static void RenderSymbol(RenderState state, SymbolExpression sym) {
        state.Builder.Append(sym.Value);
    }
}