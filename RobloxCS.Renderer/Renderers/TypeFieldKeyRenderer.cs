using RobloxCS.AST.Types;

namespace RobloxCS.Renderer.Renderers;

public class TypeFieldKeyRenderer : IRenderer<TypeFieldKey> {
    public void Render(RenderState state, TypeFieldKey node) {
        switch (node) {
            case NameTypeFieldKey name: {
                state.Builder.Append(name.Name);

                break;
            }

            case IndexSignatureTypeFieldKey index: {
                state.Builder.Append('[');

                var typeRenderer = state.GetRenderer<TypeInfo>();
                typeRenderer.Render(state, index.Inner);

                state.Builder.Append(']');

                break;
            }
        }
    }
}