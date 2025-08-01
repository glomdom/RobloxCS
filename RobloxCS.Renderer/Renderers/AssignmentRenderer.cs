using RobloxCS.AST.Statements;

namespace RobloxCS.Renderer.Renderers;

public class AssignmentRenderer : IRenderer<Assignment> {
    public void Render(RenderState state, Assignment node) {
        var vars = string.Join(", ", node.Vars);
        var expressions = string.Join(", ", node.Expressions);
        
        state.AppendIndentedLine($"{vars} = {expressions}");
    }
}