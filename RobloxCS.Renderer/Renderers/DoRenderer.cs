using RobloxCS.AST;
using RobloxCS.AST.Statements;

namespace RobloxCS.Renderer.Renderers;

public class DoRenderer : IRenderer<DoStatement> {
    public void Render(RenderState state, DoStatement node) {
        state.AppendIndentedLine("do");

        var renderer = state.GetRenderer<Block>();
        renderer.Render(state, node.Block);
        
        state.AppendIndentedLine("end");
    }
}