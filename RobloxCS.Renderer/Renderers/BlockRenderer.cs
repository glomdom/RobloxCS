using RobloxCS.AST;

namespace RobloxCS.Renderer.Renderers;

public class BlockRenderer : IRenderer<Block> {
    public void Render(RenderState state, Block node) {
        state.PushIndent();

        foreach (var stmt in node.Statements) {
            state.Builder.Append(state.Indent);

            var renderer = state.GetRenderer(stmt.GetType());
            renderer.Render(state, stmt);
        }

        state.PopIndent();
    }
}