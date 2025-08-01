using RobloxCS.AST.Types;

namespace RobloxCS.Renderer.Renderers;

public class TypeDeclarationRenderer : IRenderer<TypeDeclaration> {
    public void Render(RenderState state, TypeDeclaration node) {
        state.AppendIndented("type ");
        state.Builder.Append(node.Name);
        state.Builder.Append(" = ");

        var typeRenderer = state.GetRenderer<TypeInfo>();
        typeRenderer.Render(state, node.DeclareAs);
        
        state.Builder.AppendLine();
    }
}