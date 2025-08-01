using RobloxCS.AST.Statements;

namespace RobloxCS.Renderer.Renderers;

public class LocalAssignmentRenderer : IRenderer<LocalAssignment> {
    public void Render(RenderState state, LocalAssignment node) {
        state.Builder.Append(state.Indent);
        state.Builder.Append("local ");
        state.Builder.Append(string.Join(", ", node.Names));

        if (node.Expressions.Count == 0) {
            state.Builder.AppendLine();

            return;
        }

        state.Builder.Append(" = ");
        state.Builder.AppendLine(string.Join(", ", node.Expressions));
    }
}