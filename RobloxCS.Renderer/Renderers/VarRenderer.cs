using RobloxCS.AST;

namespace RobloxCS.Renderer.Renderers;

public class VarRenderer : IRenderer<Var> {
    public void Render(RenderState state, Var node) {
        switch (node) {
            case VarExpression varExpr: {
                var renderer = state.GetRenderer<Expression>();
                renderer.Render(state, varExpr.Expression);

                break;
            }

            case VarName varName: {
                state.Builder.Append(varName.Name);

                break;
            }
        }
    }
}