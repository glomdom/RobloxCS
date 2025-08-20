using RobloxCS.AST.Expressions;
using RobloxCS.AST.Prefixes;

namespace RobloxCS.Renderer.Renderers;

public class PrefixRenderer : IRenderer<Prefix> {
    public void Render(RenderState state, Prefix node) {
        switch (node) {
            case NamePrefix name: {
                state.Builder.Append(name.Name);

                break;
            }

            case ExpressionPrefix expression: {
                var expressionRenderer = state.GetRenderer<Expression>();
                expressionRenderer.Render(state, expression.Expression);

                break;
            }

            default: throw new Exception($"Unhandled prefix: {node.GetType().Name}");
        }
    }
}