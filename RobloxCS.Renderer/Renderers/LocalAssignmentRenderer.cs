using RobloxCS.AST.Statements;
using RobloxCS.AST.Types;

namespace RobloxCS.Renderer.Renderers;

public class LocalAssignmentRenderer : IRenderer<LocalAssignment> {
    public void Render(RenderState state, LocalAssignment node) {
        state.Builder.Append(state.Indent);
        state.Builder.Append("local ");

        var typeRenderer = state.GetRenderer<TypeInfo>();
        var nameTypes = node.Names.Zip(node.Types);
        foreach (var (name, type) in nameTypes) {
            state.Builder.Append(name.Value);
            state.Builder.Append(": ");
            typeRenderer.Render(state, type);
        }

        if (node.Expressions.Count == 0) {
            state.Builder.AppendLine();

            return;
        }

        state.Builder.Append(" = ");
        state.Builder.AppendLine(string.Join(", ", node.Expressions));
    }
}