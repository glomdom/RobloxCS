using RobloxCS.AST;

namespace RobloxCS.Renderer.Renderers;

public interface IRenderer<in T> where T : AstNode {
    public void Render(RenderState state, T node);
}

public interface IRenderer {
    public void Render(RenderState state, AstNode node);
}