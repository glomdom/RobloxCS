using RobloxCS.AST;
using RobloxCS.Renderer.Renderers;

namespace RobloxCS.Renderer;

public class RendererAdapter<T> : IRenderer, IRenderer<T> where T : AstNode {
    private readonly IRenderer<T> _tRenderer;

    public RendererAdapter(IRenderer<T> renderer) {
        _tRenderer = renderer;
    }
    
    public void Render(RenderState state, AstNode node) {
        if (node is not T tNode) {
            throw new InvalidCastException($"Expected {typeof(T).Name} got {node.GetType().Name}");
        }

        _tRenderer.Render(state, tNode);
    }

    public void Render(RenderState state, T node) {
        _tRenderer.Render(state, node);
    }
}