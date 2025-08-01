using RobloxCS.AST.Statements;

namespace RobloxCS.Renderer.Renderers;

public class ReturnRenderer : IRenderer<Return> {
    public void Render(RenderState state, Return node) {
        state.Builder.Append(state.Indent);
        state.Builder.AppendLine($"return {string.Join(", ", node.Returns)}");
    }
}