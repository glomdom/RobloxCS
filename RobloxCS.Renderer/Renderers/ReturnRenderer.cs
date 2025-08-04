using RobloxCS.AST.Statements;

namespace RobloxCS.Renderer.Renderers;

public class ReturnRenderer : IRenderer<Return> {
    public void Render(RenderState state, Return node) {
        state.AppendIndent();

        state.Builder.Append("return ");
        state.RenderPunctuated(node.Returns, state);
    }
}