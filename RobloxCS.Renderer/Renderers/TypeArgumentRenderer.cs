using RobloxCS.AST.Types;

namespace RobloxCS.Renderer.Renderers;

public sealed class TypeArgumentRenderer : IRenderer<TypeArgument> {
    public void Render(RenderState state, TypeArgument node) {
        var typeRenderer = state.GetRenderer<TypeInfo>();

        if (node.HasName) {
            state.Builder.Append(node.Name);
            state.Builder.Append(": ");
        }

        typeRenderer.Render(state, node.TypeInfo);
    }
}