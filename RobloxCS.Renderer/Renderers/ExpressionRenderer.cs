using RobloxCS.AST;
using RobloxCS.AST.Expressions;

namespace RobloxCS.Renderer.Renderers;

public class ExpressionRenderer : IRenderer<Expression> {
    public void Render(RenderState state, Expression node) {
        if (node is BooleanExpression expr) {
            RenderBoolean(state, expr);
        }
    }

    private static void RenderBoolean(RenderState state, BooleanExpression expr) {
        state.Builder.Append(expr.Value ? "true" : "false");
    }
}